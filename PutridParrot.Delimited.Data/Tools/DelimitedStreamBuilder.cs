using System;
using System.Collections.Generic;
using System.Linq;

namespace PutridParrot.Delimited.Data.Tools
{
    public class DelimitedStreamBuilder
    {
        private readonly List<DelimitedStreamReader> _readers;

        private readonly List<Func<IList<string>, bool>> _filters;
        private readonly List<Func<IList<string>, IList<string>>> _processors;

        private DelimitedStreamBuilder()
        {
            _readers = new List<DelimitedStreamReader>();
            _filters = new List<Func<IList<string>, bool>>();
            _processors = new List<Func<IList<string>, IList<string>>>();
        }

        public static DelimitedStreamBuilder New() =>
            new();

        public static DelimitedStreamBuilder New(DelimitedStreamReader reader)
        {
            var instance = New();
            instance.Concat(reader);
            return instance;
        }

        public DelimitedStreamBuilder Concat(DelimitedStreamReader reader)
        {
            _readers.Add(reader);
            return this;
        }

        public DelimitedStreamBuilder Select(Func<IList<string>, IList<string>> processor)
        {
            _processors.Add(processor);
            return this;
        }

        public DelimitedStreamBuilder Where(Func<IList<string>, bool> filter)
        {
            _filters.Add(filter);
            return this;
        }

        private bool Filter(IList<string> fields) =>
            _filters.Any(f => f(fields));

        private IList<string> Process(IList<string> fields) =>
            _processors.Aggregate(fields, (current, processor) => processor(current));

        public void Write(DelimitedStreamWriter writer, int skip = 0)
        {
            foreach (var reader in _readers)
            {
                for (var i = 0; i < skip; i++)
                {
                    reader.ReadLine();
                }

                while (reader.ReadLine() is { } fields)
                {
                    if (!Filter(fields))
                    {
                        writer.WriteLine(Process(fields));
                    }
                }
            }
            writer.Flush();
        }
    }

}
