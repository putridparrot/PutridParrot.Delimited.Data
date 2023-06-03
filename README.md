## PutridParrot.Delimited.Data

[![Build](https://github.com/putridparrot/PutridParrot.Delimited.Data/actions/workflows/build.yml/badge.svg)](https://github.com/putridparrot/PutridParrot.Delimited.Data/actions/workflows/build.yml)
[![NuGet version (PutridParrot.Delimited.Data)](https://img.shields.io/nuget/v/PutridParrot.Delimited.Data.svg?style=flat-square)](https://www.nuget.org/packages/PutridParrot.Delimited.Data/)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/putridparrot/PutridParrot.Delimited.Data/blob/master/LICENSE.md)
[![GitHub Releases](https://img.shields.io/github/release/putridparrot/PutridParrot.Delimited.Data.svg)](https://github.com/putridparrot/PutridParrot.Delimited.Data/releases)
[![GitHub Issues](https://img.shields.io/github/issues/putridparrot/PutridParrot.Delimited.Data.svg)](https://github.com/putridparrot/PutridParrot.Delimited.Data/issues)


PutridParrot.Delimited.Data was designed to allow the user to interact CSV, TSV etc. data. 

Everything happens within the Delimited.Data.Specializations namespace where we have wrapper specializations for Csv and Tsv 
but you can easily use or create your own on top of the PutridParrot.Delimited.Data class.

Let's start by looking at the most obvious of the specializations - those which interact with CSV as these are really what started this library off.

We have several ways of dealing with the parsing of delimited data, from a low level reader to a version which serializes/deserializes to objects and 
finally a dynamic way of using CSV which allows me to use LINQ to make the CSV queryable.

### CsvReader/CsvWriter

Inheriting from the DelimitedStreamReader/DelimitedStreamWriter these offer basic functionality allowing you to read a line of delimited data at a time or writing a line at a time. These classes will parse the data and return an IList of string items, in the case of the reader. The writer takes an IEnumerable of strings and writes them in the format of the delimited data. 

```
var reader = new CsvReader(stream);

IList<string> fields;
while((fields = reader.ReadLine()) != null)

{ 
   // do something with the fields
}
```

Both async and non-async read and write methods exist.

### CsvSerializer

This is a convenience class for using the DelimitedSerializer. The CsvSerializer can serialize/deserialize objects to/from the data. It can use either attributes on the serializable object or can be supplied with "mappings" which tell it what to map from/to.

### CsvEnumerator

The next method of working with delimited data is the DelimitedDataEnumerator. The Csv specialization again , is just a convenience for working with the Csv data options.

This method of interacting with delimited data will only deserialize data as an IEnumerable. This will return dynamic data (DynamicObject). This allows us to interact with the field names via a couple of mechanisms. 

The first is based upon using the heading name from the data the second via the column index. The key use of this class is how the returned dynamic data can be used from LINQ, thus allowing us to query the data.

## Code Generation

In some cases we need to create an object or it's mappings (for use with the CsvSerializer) from the data. To save having to write the code for this we can use the DelimitedCodeGenerator application to generate the .cs file representing the serialization object.

### Library license

The library is available under the MIT license. See the [License file][1] in the GitHub repository.

  [1]: https://github.com/putridparrot/Delimited.Data/blob/master/LICENSE
