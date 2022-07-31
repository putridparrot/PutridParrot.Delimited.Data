## Delimited.Data

[![Build](https://github.com/putridparrot/PutridParrot.Delimited.Data/actions/workflows/build.yml/badge.svg)](https://github.com/putridparrot/PutridParrot.Delimited.Data/actions/workflows/build.yml)

Delimited.Data was designed to allow me to interact CSV, TSV etc. data. There are specializations within the Delimited.Data.Specializations assembly for Csv and Tsv but you can easily create your own on top of the Delimited.Data class.

Let's start by looking at the most obvious specializations - those which interact with CSV.

I've created several ways of dealing with the parsing of delimited data, from a low level reader to a version which serializes/deserializes to objects and finally a dynamic way of using CSV which allows me to use LINQ to make the CSV querable.

### CsvReader/CsvWriter

Inheriting from the DelimitedStreamReader/DelimitedStreamWriter these offer basic functionality allowing you to read a line of delimited data at a time or writing a line at a time. These classes will parse the data and return an IList of string items, in the case of the reader. The writer takes an IEnumerable of strings and writes them in the format of the delimited data. In this case reading and writing CSV.

```
var reader = new CsvReader(stream);

IList<string> fields;
while((fields = reader.ReadLine()) != null)

{ 
   // do something with the fields
}
```
### CsvSerializer

This is a convenience class for using the DelimitedSerializer. The CsvSerializer can serialize/deserialize objects to/from the data. It can use either attributes on the serializable object or can be supplied with "mappings" which tell it what to map from/to.

### CsvEnumerator

The next method of working with delimited data is the DelimitedDataEnumerator. The Csv specialization again , is just a convenience for working with the Csv data options.

This method of interacting with delimited data will only deserialize data and instead of to an IList or a pre-defined object, it will return dynamic data. This allows us to interact with the field names via a couple of mechanism. The first are based upon using the heading name from the data the second via the column index.

The resultant IEnumerable is also useable from LINQ thus allowing us to query the data.

## Code Generation

In some cases we need to create an object or it's mappings (for use with the CsvSerializer) from the data. To save having to write the code for this we can use the DelimitedCodeGenerator application to generate the .cs file representing the serialization object.

## Other Tools

### DelimitedStreamConnector

DelimitedStreamConnector is a very simple class which simply handles the connecting of two delimited streams, a reader and writer are supplied and the Pipe method will simply 
 transform data from one stream reader into the other stream writer.

### Library license

The library is available under the MIT license. See the [License file][1] in the GitHub repository.

  [1]: https://github.com/putridparrot/Delimited.Data/blob/master/LICENSE
