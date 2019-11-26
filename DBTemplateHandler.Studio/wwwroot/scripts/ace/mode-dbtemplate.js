﻿//Highlight rules
ace.define("ace/mode/dbtemplate_highlight_rules", ["require", "exports", "module", "ace/lib/oop", "ace/mode/text_highlight_rules"] , function (require, exports, module) {
    "use strict";
    // https://ace.c9.io/tool/mode_creator.html
    var oop = require("../lib/oop");
    var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

    var DbTemplateHighlightRules = function () {

        // regexp must not have capturing parentheses. Use (?:) instead.
        // regexps are ordered -> the first match is used
        this.$rules = { "start": [{ "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:INDEX", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:INDEX", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:TYPE", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:AUTO\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:AUTO\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:NULL\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:PRIMARY\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:PRIMARY\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:NULL:CURRENT:IS:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:NULL:FOREACH:CURRENT:IS:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:INDEX", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:PRIMARY:NOT:NULL:CURRENT:INDEX", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:INDEX", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:INDEX", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:AUTO:FOREACH\\[", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:FOREACH\\[", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH\\[", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:NULL:FOREACH\\[", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH\\[", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH\\[", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:CURRENT:NAME", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:CURRENT:NAME", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:TABLE:FOREACH\\[", "next": "end-context" }, { "token": "keyword", "regex": "\\{:TDB:FUNCTION:FIRST:CHARACTER:TO:UPPER:CASE\\(", "next": "end-context" }], "end-context": [{ "token": "keyword", "regex": "::}", "next": "start" }, { "token": "keyword", "regex": "\\)::}", "next": "start" }, { "token": "keyword", "regex": "\\):::}", "next": "start" }, { "token": "keyword", "regex": "\\)KEY:AUTO:::}", "next": "start" }, { "token": "keyword", "regex": "\\)KEY:NOT:AUTO:::}", "next": "start" }, { "token": "keyword", "regex": "\\)KEY:NOT:PRIMARY:::}", "next": "start" }, { "token": "keyword", "regex": "\\)KEY:PRIMARY:::}", "next": "start" }, { "token": "keyword", "regex": "]::}", "next": "start" }] };
    };

    oop.inherits(DbTemplateHighlightRules, TextHighlightRules);

    exports.DbTemplateHighlightRules = DbTemplateHighlightRules;

});


ace.define("ace/mode/folding/dbtemplate", ["require", "exports", "module", "ace/lib/oop", "ace/range", "ace/mode/folding/fold_mode"], function (require, exports, module) {
    "use strict";

    var oop = require("../../lib/oop");
    var Range = require("../../range").Range;
    var BaseFoldMode = require("./fold_mode").FoldMode;

    var FoldMode = exports.FoldMode = function (commentRegex) {
        if (commentRegex) {
            this.foldingStartMarker = new RegExp(
                this.foldingStartMarker.source.replace(/\|[^|]*?$/, "|" + commentRegex.start)
            );
            this.foldingStopMarker = new RegExp(
                this.foldingStopMarker.source.replace(/\|[^|]*?$/, "|" + commentRegex.end)
            );
        }
    };
    oop.inherits(FoldMode, BaseFoldMode);

    (function () {

        this.foldingStartMarker = /([\{\[\(])[^\}\]\)]*$|^\s*(\/\*)/;
        this.foldingStopMarker = /^[^\[\{\(]*([\}\]\)])|^[\s\*]*(\*\/)/;
        this.singleLineBlockCommentRe = /^\s*(\/\*).*\*\/\s*$/;
        this.tripleStarBlockCommentRe = /^\s*(\/\*\*\*).*\*\/\s*$/;
        this.startRegionRe = /^\s*(\/\*|\/\/)#?region\b/;
        this._getFoldWidgetBase = this.getFoldWidget;
        this.getFoldWidget = function (session, foldStyle, row) {
            var line = session.getLine(row);

            if (this.singleLineBlockCommentRe.test(line)) {
                if (!this.startRegionRe.test(line) && !this.tripleStarBlockCommentRe.test(line))
                    return "";
            }

            var fw = this._getFoldWidgetBase(session, foldStyle, row);

            if (!fw && this.startRegionRe.test(line))
                return "start"; // lineCommentRegionStart

            return fw;
        };

        this.getFoldWidgetRange = function (session, foldStyle, row, forceMultiline) {
            var line = session.getLine(row);

            if (this.startRegionRe.test(line))
                return this.getCommentRegionBlock(session, line, row);

            var match = line.match(this.foldingStartMarker);
            if (match) {
                var i = match.index;

                if (match[1])
                    return this.openingBracketBlock(session, match[1], row, i);

                var range = session.getCommentFoldRange(row, i + match[0].length, 1);

                if (range && !range.isMultiLine()) {
                    if (forceMultiline) {
                        range = this.getSectionRange(session, row);
                    } else if (foldStyle != "all")
                        range = null;
                }

                return range;
            }

            if (foldStyle === "markbegin")
                return;

            var match = line.match(this.foldingStopMarker);
            if (match) {
                var i = match.index + match[0].length;

                if (match[1])
                    return this.closingBracketBlock(session, match[1], row, i);

                return session.getCommentFoldRange(row, i, -1);
            }
        };

        this.getSectionRange = function (session, row) {
            var line = session.getLine(row);
            var startIndent = line.search(/\S/);
            var startRow = row;
            var startColumn = line.length;
            row = row + 1;
            var endRow = row;
            var maxRow = session.getLength();
            while (++row < maxRow) {
                line = session.getLine(row);
                var indent = line.search(/\S/);
                if (indent === -1)
                    continue;
                if (startIndent > indent)
                    break;
                var subRange = this.getFoldWidgetRange(session, "all", row);

                if (subRange) {
                    if (subRange.start.row <= startRow) {
                        break;
                    } else if (subRange.isMultiLine()) {
                        row = subRange.end.row;
                    } else if (startIndent == indent) {
                        break;
                    }
                }
                endRow = row;
            }

            return new Range(startRow, startColumn, endRow, session.getLine(endRow).length);
        };
        this.getCommentRegionBlock = function (session, line, row) {
            var startColumn = line.search(/\s*$/);
            var maxRow = session.getLength();
            var startRow = row;

            var re = /^\s*(?:\/\*|\/\/|--)#?(end)?region\b/;
            var depth = 1;
            while (++row < maxRow) {
                line = session.getLine(row);
                var m = re.exec(line);
                if (!m) continue;
                if (m[1]) depth--;
                else depth++;

                if (!depth) break;
            }

            var endRow = row;
            if (endRow > startRow) {
                return new Range(startRow, startColumn, endRow, line.length);
            }
        };

    }).call(FoldMode.prototype);

});




ace.define("ace/mode/dbtemplate", ["require", "exports", "module", "ace/lib/oop", "ace/mode/text", "ace/mode/db_template_highlight_rules", "ace/mode/folding/db_template"], function (require, exports, module) {
    "use strict";

    var oop = require("../lib/oop");
    var TextMode = require("./text").Mode;
    var DockerfileHighlightRules = require("./dbtemplate_highlight_rules").DbTemplateHighlightRules;
    var DBTemplateFoldMode = require("./folding/dbtemplate").FoldMode;

    var Mode = function () {
        TextMode .call(this);

        this.HighlightRules = DockerfileHighlightRules;
        this.foldingRules = new DBTemplateFoldMode();
    };
    oop.inherits(Mode, TextMode );

    (function () {
        this.$id = "ace/mode/dbtemplate";
    }).call(Mode.prototype);

    exports.Mode = Mode;
});

(function () {
    ace.require(["ace/mode/dbtemplate"], function (m) {
        if (typeof module == "object" && typeof exports == "object" && module) {
            module.exports = m;
        }
    });
})();