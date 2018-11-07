# Presentations

## HTML slides

To generate usable slides in html format (Linux or Cygwin):

```sh
./make.sh elmish.md > elmish.html
./make.sh rest.md > rest.html
```

## PDF examples

To generate PDF from the code examples, you need to install:

* The [Tectonic](https://tectonic-typesetting.github.io/en-US/) LaTeX engine
* [pandoc](https://pandoc.org/)
* The [Eisvogel](https://github.com/Wandmalfarbe/pandoc-latex-template)
  pandoc template in ``~/.pandoc/templates``

Then run:
```sh
./eisvogel Fs-code.md
./eisvogel Toodeloo-code.md
```
