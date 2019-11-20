define(function (require, exports, module) {
    "use strict";
    // https://ace.c9.io/tool/mode_creator.html
    var oop = require("../lib/oop");
    var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

    var MyNewHighlightRules = function () {

        // regexp must not have capturing parentheses. Use (?:) instead.
        // regexps are ordered -> the first match is used
        this.$rules = {
            "start": [
                {
                    token: "keyword", // String, Array, or Function: the CSS token to apply
                    regex: "\{\:TDB\:", // String or RegExp: the regexp to match
                    next: "tdb"   // [Optional] String: next state to enter
                }
            ],
            "tdb": [
                {
                    token: "keyword", // String, Array, or Function: the CSS token to apply
                    regex: "COUCOU", // String or RegExp: the regexp to match
                    next: "tdb-end"   // [Optional] String: next state to enter
                }
            ],
            "tdb-end": [
                {
                    token: "keyword", // String, Array, or Function: the CSS token to apply
                    regex: "(\\(|\\[)", // String or RegExp: the regexp to match
                    next: "start"   // [Optional] String: next state to enter
                }
            ]
        };
    };

    oop.inherits(MyNewHighlightRules, TextHighlightRules);

    exports.MyNewHighlightRules = MyNewHighlightRules;

});