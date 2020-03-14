//Highlight rules
ace.define("ace/mode/dbtemplate_highlight_rules", ["require", "exports", "module", "ace/lib/oop", "ace/mode/text_highlight_rules"], function (require, exports, module) {
    "use strict";
    // https://ace.c9.io/tool/mode_creator.html
    var oop = require("../lib/oop");
    var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

    var DbTemplateHighlightRules = function () {

        // regexp must not have capturing parentheses. Use (?:) instead.
        // regexps are ordered -> the first match is used
        this.$rules = {"start":[{"token":"keyword","regex":"\\{:TDB:PREPROCESSOR:MAPPING:DECLARE\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:INDEX","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:INDEX","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:TYPE","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:MAX:SIZE","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:AUTO\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:AUTO\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:NULL\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NULL\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:PRIMARY\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:PRIMARY\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:NULL:CURRENT:IS:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:NULL:FOREACH:CURRENT:IS:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:INDEXED:FOREACH:CURRENT:IS:FIRST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:INDEXED:FOREACH:CURRENT:IS:LAST:COLUMN\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:INDEX","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:PRIMARY:NOT:NULL:CURRENT:INDEX","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:INDEX","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:INDEX","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:AUTO:FOREACH\\[","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:FOREACH\\[","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:INDEXED:FOREACH\\[","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:INDEXED:FOREACH\\[","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH\\[","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:NULL:FOREACH\\[","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH\\[","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:COLUMN:PRIMARY:FOREACH\\[","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:CURRENT:NAME","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:CURRENT:SCHEMA","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:CURRENT:WHEN:HAS:AUTO\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:CURRENT:WHEN:HAS:INDEX\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:CURRENT:WHEN:HAS:NOT:INDEX\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:CURRENT:NAME","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:CURRENT:CONNECTION:STRING","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:TABLE:FOREACH\\[","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:FUNCTION:FIRST:CHARACTER:TO:UPPER:CASE\\(","next":"end-context"},{"token":"keyword","regex":"\\{:TDB:FUNCTION:REPLACE\\(","next":"end-context"}],"end-context":[{"token":"keyword","regex":"\\):PREPROCESSOR:}","next":"start"},{"token":"keyword","regex":"::}","next":"start"},{"token":"keyword","regex":"\\)::}","next":"start"},{"token":"keyword","regex":"\\):::}","next":"start"},{"token":"keyword","regex":"\\)KEY:AUTO:::}","next":"start"},{"token":"keyword","regex":"\\)KEY:NOT:AUTO:::}","next":"start"},{"token":"keyword","regex":"\\)KEY:NOT:PRIMARY:::}","next":"start"},{"token":"keyword","regex":"\\)KEY:PRIMARY:::}","next":"start"},{"token":"keyword","regex":"]::}","next":"start"},{"token":"keyword","regex":"]\\)::}","next":"start"}]};
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
    var langTools = require("ace/ext/language_tools");

    var Mode = function () {
        TextMode.call(this);

        this.HighlightRules = DockerfileHighlightRules;
        this.foldingRules = new DBTemplateFoldMode();
    };
    oop.inherits(Mode, TextMode);

    (function () {
        this.$id = "ace/mode/dbtemplate";
    }).call(Mode.prototype);

    var dbTemplateSemantics = [{"startContext":"{:TDB:PREPROCESSOR:MAPPING:DECLARE(","endContext":"):PREPROCESSOR:}","handler":"MappingDeclarePreprcessorContextHandler","category":"unknown","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:INDEX","endContext":"::}","handler":"AutoColumnIndexColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:INDEX","endContext":"::}","handler":"ColumnIndexColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME","endContext":"::}","handler":"ColumnNameColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE(","endContext":")::}","handler":"ColumnValueConvertTypeColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:TYPE","endContext":"::}","handler":"ColumnValueTypeColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:MAX:SIZE","endContext":"::}","handler":"ColumnValueMaxSizeColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:FIRST:COLUMN(","endContext":"):::}","handler":"IsAutoColumnAFirstAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:LAST:COLUMN(","endContext":"):::}","handler":"IsAutoColumnALastAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(","endContext":"):::}","handler":"IsAutoColumnNotAFirstAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(","endContext":"):::}","handler":"IsAutoColumnNotALastAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:FIRST:COLUMN(","endContext":"):::}","handler":"IsColumnAFirstColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:LAST:COLUMN(","endContext":"):::}","handler":"IsColumnALastColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:AUTO(","endContext":")KEY:AUTO:::}","handler":"IsColumnAutoGeneratedValueColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(","endContext":"):::}","handler":"IsColumnNotAFirstColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(","endContext":"):::}","handler":"IsColumnNotALastColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:AUTO(","endContext":")KEY:NOT:AUTO:::}","handler":"IsColumnNotAutoGeneratedValueColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:NULL(","endContext":"):::}","handler":"IsColumnNotNullValueColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NULL(","endContext":"):::}","handler":"IsColumnNullValueColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:PRIMARY(","endContext":")KEY:NOT:PRIMARY:::}","handler":"IsColumnNotPrimaryKeyColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:PRIMARY(","endContext":")KEY:PRIMARY:::}","handler":"IsColumnPrimaryKeyColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:FIRST:COLUMN(","endContext":"):::}","handler":"IsNotAutoColumnAFirstAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:LAST:COLUMN(","endContext":"):::}","handler":"IsNotAutoColumnALastAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(","endContext":"):::}","handler":"IsNotAutoColumnNotAFirstAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(","endContext":"):::}","handler":"IsNotAutoColumnNotALastAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:NULL:CURRENT:IS:FIRST:COLUMN(","endContext":"):::}","handler":"IsNotNullColumnAFirstAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:NULL:FOREACH:CURRENT:IS:LAST:COLUMN(","endContext":"):::}","handler":"IsNotNullColumnALastAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN(","endContext":"):::}","handler":"IsNotPrimaryColumnAFirstAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN(","endContext":"):::}","handler":"IsNotPrimaryColumnALastAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(","endContext":"):::}","handler":"IsNotPrimaryColumnNotAFirstAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(","endContext":"):::}","handler":"IsNotPrimaryColumnNotALastAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN(","endContext":"):::}","handler":"IsPrimaryColumnAFirstPrimaryColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN(","endContext":"):::}","handler":"IsPrimaryColumnALastPrimaryColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(","endContext":"):::}","handler":"IsPrimaryColumnNotAFirstAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(","endContext":"):::}","handler":"IsPrimaryColumnNotALastAutoColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:INDEXED:FOREACH:CURRENT:IS:FIRST:COLUMN(","endContext":"):::}","handler":"IsIndexedColumnAFirstIndexedColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:INDEXED:FOREACH:CURRENT:IS:LAST:COLUMN(","endContext":"):::}","handler":"IsIndexedColumnALastIndexedColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:INDEX","endContext":"::}","handler":"NotAutoColumnIndexColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:COLUMN:PRIMARY:NOT:NULL:CURRENT:INDEX","endContext":"::}","handler":"NotNullColumnIndexColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:INDEX","endContext":"::}","handler":"NotPrimaryColumnIndexColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:INDEX","endContext":"::}","handler":"PrimaryColumnIndexColumnContextHandler","category":"column","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:COLUMN:AUTO:FOREACH[","endContext":"]::}","handler":"ForEachAutoGeneratedValueColumnTableContextHandler","category":"column-loop","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:FOREACH[","endContext":"]::}","handler":"ForEachColumnTableContextHandler","category":"column-loop","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:INDEXED:FOREACH[","endContext":"]::}","handler":"ForEachIndexedColumnTableContextHandler","category":"column-loop","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:INDEXED:FOREACH[","endContext":"]::}","handler":"ForEachNotIndexedColumnTableContextHandler","category":"column-loop","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH[","endContext":"]::}","handler":"ForEachNotAutoGeneratedValueColumnTableContextHandler","category":"column-loop","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:NULL:FOREACH[","endContext":"]::}","handler":"ForEachNotNullColumnTableContextHandler","category":"column-loop","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH[","endContext":"]::}","handler":"ForEachNotPrimaryKeyColumnTableContextHandler","category":"column-loop","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:COLUMN:PRIMARY:FOREACH[","endContext":"]::}","handler":"ForEachPrimaryKeyColumnTableContextHandler","category":"column-loop","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:CURRENT:NAME","endContext":"::}","handler":"TableNameTableContextHandler","category":"table","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:CURRENT:SCHEMA","endContext":"::}","handler":"TableSchemaTableContextHandler","category":"table","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:CURRENT:WHEN:HAS:AUTO(","endContext":")::}","handler":"WhenHasAutoGeneratedValueColumnTableContextHandler","category":"table","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:CURRENT:WHEN:HAS:INDEX(","endContext":")::}","handler":"WhenHasIndexColumnTableContextHandler","category":"table","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:TABLE:CURRENT:WHEN:HAS:NOT:INDEX(","endContext":")::}","handler":"WhenHasNotIndexColumnTableContextHandler","category":"table","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:CURRENT:NAME","endContext":"::}","handler":"DatabaseNameDatabaseContextHandler","category":"database","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:CURRENT:CONNECTION:STRING","endContext":"::}","handler":"ConnectionStringDatabaseContextHandler","category":"database","isStartContextAndEndContextAnEntireWord":true},{"startContext":"{:TDB:TABLE:FOREACH[","endContext":"]::}","handler":"ForEachTableDatabaseContextHandler","category":"table-loop","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:FUNCTION:FIRST:CHARACTER:TO:UPPER:CASE(","endContext":")::}","handler":"FirstLetterToUpperCaseFunctionTemplateHandler","category":"function","isStartContextAndEndContextAnEntireWord":false},{"startContext":"{:TDB:FUNCTION:REPLACE(","endContext":"])::}","handler":"ReplaceWithFunctionTemplateHandler","category":"function","isStartContextAndEndContextAnEntireWord":false}];

    var entireWordSemantic = dbTemplateSemantics.filter(entry => { return entry.isStartContextAndEndContextAnEntireWord == true });
    var entireWord = entireWordSemantic.map(entry => entry.startContext + entry.endContext);
    var contextWordSemantic = dbTemplateSemantics.filter(entry => { return entry.isStartContextAndEndContextAnEntireWord == false });
    var startContextWords = contextWordSemantic.map(entry => entry.startContext);
    var endContextWords = contextWordSemantic.map(entry => entry.endContext);
    var words = entireWord.concat(startContextWords).concat(endContextWords);
    var completion = words.map(word => {
        return {
            caption: word,
            value: word,
            score: 100,
            meta: "dbtemplate"
        }
    }
    );
    var completer = {
        getCompletions: function (editor, session, pos, prefix, callback) {
            callback(null, completion);
        }
    };
    langTools.addCompleter(completer);
    exports.Mode = Mode;
});



(function () {
    ace.require(["ace/mode/dbtemplate"], function (m) {
        if (typeof module == "object" && typeof exports == "object" && module) {
            module.exports = m;
        }
    });
})();