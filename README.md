# HSharp

[![NuGet version (Newtonsoft.Json)](https://img.shields.io/nuget/v/Aiursoft.HSharp.svg?style=flat-square)](https://www.nuget.org/packages/Aiursoft.HSharp/)
[![Join the chat at https://gitter.im/AnduinHSharp/Lobby](https://badges.gitter.im/AnduinHSharp/Lobby.svg)](https://gitter.im/AnduinHSharp/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Build Status](https://travis-ci.org/Anduin2017/HSharp.svg?branch=master)](https://travis-ci.org/Anduin2017/HSharp)
[![Coverage Status](https://coveralls.io/repos/github/Anduin2017/HSharp/badge.svg?branch=master)](https://coveralls.io/github/Anduin2017/HSharp?branch=master)
[![Issues](https://img.shields.io/github/issues/Anduin2017/HSharp.svg)](https://github.com/Anduin2017/HSharp/issues)

HSharp is a library used to analyse markup language like HTML easily and fastly.

HSharp is based on .NET Standard `2.0` and supports .NET Framework, .NET Core and Xamarin.

Current version: `2.1.0`

## Only Two Functions

* Deserialize and analyse HTML
* Build HTML using C#

## How to install

### Using Nuget

To install Aiursoft.HSharp, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)

````bash
PM> Install-Package Aiursoft.HSharp
````

### Using .NET CLI tool

To install Aiursoft.HSharp, run the following command in any console:

````bash
dotnet add package Aiursoft.HSharp
````

## Examples

### Deserialize HTML

Input some HTML and get the DOM of it.

````csharp
var newDocument = HtmlConvert.DeserializeHtml($@"
<html>
    <head>
        <meta charset={"\"utf-8\""}>
        <meta name={"\"viewport\""}>
        <title>Example</title>
    </head>
<body>
    <h1>Some Text</h1>
    <table>
        <tr>OneLine</tr>
        <tr>TwoLine</tr>
        <tr>ThreeLine</tr>
    </table>
</body>
</html>");

Console.WriteLine(newDocument["html"]["head"]["meta",0].Properties["charset"]);
Console.WriteLine(newDocument["html"]["head"]["meta",1].Properties["name"]);
foreach (var line in newDocument["html"]["body"]["table"])
{
    Console.WriteLine(line.Son);
}
````

Output:

````html
utf-8
viewport
OneLine
TwoLine
ThreeLine
````

### Build HTML

Create a simple HDoc and add some children to its body.

````CSharp
var document = new HDoc(DocumentOptions.BasicHTML);
document["html"]["body"].AddChild("div");
document["html"]["body"]["div"].AddChild("a", new HProp("href", "/#"));
var result = document.GenerateHTML();
````

Output:

````html
<html>
<head>
    <meta charset="utf-8"></meta>
    <title>
        Example
    </title>
</head>
<body>
    <div>
        <a href="/#"></a>
    </div>
</body>
</html>
````

**HSharp can also operate other Markup language like XML and XAML**
