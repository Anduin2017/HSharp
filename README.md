# HSharp

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/anduin/hsharp/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/anduin/hsharp/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/anduin/hsharp/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/anduin/hsharp/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/anduin/hsharp/-/pipelines)
[![NuGet version)](https://img.shields.io/nuget/v/Anduin.HSharp.svg)](https://www.nuget.org/packages/Anduin.HSharp/)
[![ManHours](https://manhours.aiursoft.cn/gitlab/gitlab.aiursoft.cn/anduin/hsharp)](https://gitlab.aiursoft.cn/anduin/hsharp/-/commits/master?ref_type=heads)

HSharp is a library used to analyze markup language like HTML easily.

## Only Two Functions

* Deserialize and analyze HTML
* Build HTML using C#

## How to install

### Using Nuget

To install Anduin.HSharp, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)

````bash
PM> Install-Package Anduin.HSharp
````

### Using .NET CLI tool

To install Anduin.HSharp, run the following command in any console:

````bash
dotnet add package Anduin.HSharp
````

## Examples

### Deserialize HTML

Input some HTML and get the DOM of it.

````csharp
string exampleHtml = $@"
<html>
    <head>
        <meta charset=""utf-8"">
        <meta name=""viewport"">
        <title>Example</title>
    </head>
    <body>
        Some Text
        <table>
            <tr>OneLine</tr>
            <tr>TwoLine</tr>
            <tr>ThreeLine</tr>
        </table>
        Other Text
    </body>
</html>";
var doc = HtmlConvert.DeserializeHtml(exampleHtml);

Console.WriteLine(doc["html"]["head"]["meta",0].Properties["charset"]);
Console.WriteLine(doc["html"]["head"]["meta",1].Properties["name"]);
foreach (var line in doc["html"]["body"]["table"])
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

HSharp **can** also operate other Markup languages like XML and XAML
