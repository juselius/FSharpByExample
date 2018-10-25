#!/bin/sh

cat $@ templ.html | sed -n '1,/^<!D.*>$/{/^<!D.*>$/{p;d};H;d};/<textarea id="source">/G;p'
