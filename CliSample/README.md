# CliSample

A sample using [command-line-api](https://github.com/dotnet/command-line-api) to build a cli tool.


## How to run 

```sh
dotnet publish -c relase -r win-x64 -p:PublishSingleFile=true --self-contained false -o yourpath CliSample.csproj

cd yourpath
```

> Change the `-r` for your own os.

```sh
c:\runable>cli-sample.exe
Description:
  cli-sample is a cli sample

Usage:
  cli-sample [command] [options]

Options:
  -?, -h, --help  Show help and usage information
  -v, --version   Show version information

Commands:
  sum <numbers>  Get sum of input numbers
  len <string>   Get length of input string
  pow            Get a number raised to the specified power.
```


```sh
c:\runable>cli-sample.exe len "hello, catcher wong."
The result is 20
```

