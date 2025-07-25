# Technical Overview of CSV to Object Application

## 1. Introduction

This library processes a CSV file and returns:

- A dictionary mapping each column name to its corresponding data type
- An object representing the CSV data, where each row is a List<Dictionary<string, object>>
- The overall object is a list of these rows

## 2. Architecture

- C#
- CsvHelper
- CsvFinalObject, CsvTypeDetector, ColumnType ...      classes

## 3. Main Processing Flow

- Read raw CSV
- Convert to dictionary<string, string>
- Detect column types
- Convert values
- Store typed objects

## 3. Details

- CsvReaderConverter:
Csv content read using StreamReader
CsvHelper library reads the CSV and converts each row into a dynamic object
Each dynamic object is cast to a dictionary of <string, object> to access column-value pairs.
Each cell’s value is converted to a string (handling possible null values gracefully).
The resulting dictionary for the row is added to a list that accumulates all rows.

CSV file
   ↓
StreamReader
   ↓				   CsvHelper
IEnumerable<dynamic>
   ↓                               
IDictionary<string, object>
   ↓                               CsvReaderConverter.ReadCsv                    
List<Dictionary<string, string>>
   ↓                               CsvTyped                
List<Dictionary<string,object>>
   ↓                               CsvToDynamicObjectLib
List<CsvLine>

- ColumnsType
DetectColumnType: 
Take a column, for each possible type: if all value can be converted to this type, return this type.
MORE DETAILS
ConvertToType

List<Dictionary<string, string>>
   ↓                               ColumnsType
Dictionary<string, Type>





