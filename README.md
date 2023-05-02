# sniff

## overview
This console application was developed using .NET Core 7. The purpose of Sniff is to summarize the content of a specified directory. The tool was developed and tested on Linux. 

## long story short
During one emergency backup, I was surprised when I learned that my /dev folder contains tens of thousands of files and hundreds of folders. I wanted to know what kind of data was there. It turned out that this massive amount were Godot project files, and it also created a great opportunity to write my own tool.

## usage

### options
`-r` switch between only the specified folder / the specified folder AND all the subfolders (if present)

`-p` pagination

### commands
`types` (default) finds and shows all the file types and their counts

`duplicates` finds and shows all the duplicate files __(needs work)__

`sniff` about application __(needs to be implemented)__

### parameters
`--path` specifies base path

`--pattern` specifies search pattern ([System.IO](https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.getfiles?view=net-7.0#system-io-directory-getfiles(system-string-system-string)))

### examples in terminal
```
sniff
sniff -r
sniff -p "/home/alfonz/meta-files"
sniff -rp --path "/home/alfonz/dev/dotnet" --pattern "*.cpp"
sniff types -r

sniff duplicates -r --path "/home/alfonz/dev" --pattern "*.xml"

sniff sniff
```
