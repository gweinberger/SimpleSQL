
# SimpleSQL
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)   

A simple console SQL client for MS SQL Server that runs on Windows, Linux and MacOS.
The deployment creates a portable single file and can be used for free (no runtime needed).

## Usage
`SimpleSQL.exe ConnectionString`

## Usage examples
```
SimpleSQL.exe "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master; Database=Master;Integrated Security=True;Encrypt=False;"\"[/]")";
```
```
SimpleSQL.exe "Data Source=xxxxx.xxxxx.rds.amazonaws.com;Database=Master; User ID=xxx;Password=xxx;");
```

## Information
Username, password and database will be requested if it's not specified.

## Further work
Support of Oracle, MySQL and PostgreSQL
