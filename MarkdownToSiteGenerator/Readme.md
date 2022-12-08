# Markdown-To-Site Generator

This solution builds a static multiple-HTML-page site from a directory of markdown files that can be hosted without an application layer. The builder produces pages based on a standard Bootstrap HTML5 style, including a drop-down menu to navigate. Images, JS, and style sheets are placed in separate directories.

This solution is largely public as example of my personal code and development style. It is written from scratch in a test-driven-development oriented style, in C# 9.0 and .NET Core 7.0. 

The solution is designed to be extensible - parsing documents other than markdown, for example, or producing documents other than HTML. 

## How to Use

Build to solution in Visual Studio 17.4.2 or with the .NET Core command line tools. This will provide and executable. You can also build the unit tests.

Call the executable without arguments to see help documentation. Typically, you would simply call it like so:

```
MarkDownToSiteGenerator C:\my\directory\with\markdown_files\ C:\my\html\output\directory\
```

## Examples

You can see an example website in the Example_Input and Example_Output folders.

## Input Directory Structure

Directories can be nested as deep as you would like and will reflect the final site structure. Spaces in paths are converted to hyphens in the HTML site. The output directory must not be within the input directory, and vice versa.

### Settings

You can optionally include a config.ini file in the top directory of your input to change how the site is generated. See the examples folder for relevant settings.

## Links


### Linking between pages

To link between pages, link to the title of that file, not its path or destination
```
[the content of the link](title_of_the_that_file)
```

If your page title contains spaces, replace them with underscores in the link. For more information on titles, see Metadata in this document.

Links written in this way are checked for correctness. An exception is thrown if the page is not found.

### Linking to URLs

You can also link directly to urls, which will not be altered, in the normal way. These must begin with `/`, `//`, `https://` or `www.`:

```
[the content of the link](https://example.com)
```


### Home in the Navigation Bar

The navigation bar will link to your home page if you include the following in your config.ini file:

```
site_name=<the name of the link text you would like in the navigation bar>
home_page=<title of your home page> (not the path)
```

See Metadata in this document to understand the title. If you do not provide the `home_page` but do provide the `site_name` the link will be to `/`.


## Metadata

Add metadata to MD documents like so (including the blank line):
```
title: My page
author: Mike Dent

```

These, if any, must appear at the start of the document. They will be placed in the HTML head as meta tags. Stick to letters (a-Z) for the keys.

### Title Tag

The title metadata tag is special and used for:

1. Building link text to that file
2. `<title>` in the html head (rather than `<meta>`)

If you do not provide a title tag, the first H1 is used in its place.

It is critical that your pages never have identical titles as it makes linking impossible. Duplicate titles will result in an exception.

## Limitations

* This is not intended to be a complete solution for all use cases and only covers a subset of Markdown syntax. It is readily extensible, though.
* Solution does not support old MacOS line endings (`\r`)