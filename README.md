<div class="container"><!--!-->
        <!--!--><div class="row">
            <h1>DB Template Handler</h1>
        </div>
        <!--!--><div class="row">
            <p class="lead">
                This application aims to generate Data Access Layers agnosticly to the target language environment. To perform this purpose it will perform a jointure between three models
                </p><ul>
                    <li>Database model -&gt; A lightweight implementation of the database model which aggregate all the mandatory properties of the accessed database in order to implement a Data Access Layer</li>
                    <li>Templates -&gt; Templates which are used to implement the data access layer and any other script related generation (as example : create table sql generation script templates)</li>
                    <li>Type mapping -&gt; This is to define the mapping to perform between both environment accessing/accessed in order to implement the DAL</li>
                </ul>
            <p></p>
        </div>
		<div>
			Pre beta binary distribution : 
			<a href="https://www.maximilienzakowski.org/download/db-template-handler-pre-beta-distribution/">windows-x64</a>
		</div>
        <!--!--><div class="row">
            <img src="https://github.com/cosXsinX/DBTemplateHanlder-NetCore3/blob/master/DBTemplateHandler.Studio/wwwroot/images/DBTemplateHandlerTemplateEditorImage.png?raw=true" class="img-fluid" alt="Image">
        </div>
        <!--!--><div class="row">
            <h2>Template specific keywords</h2>
        </div>
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:INDEX::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current auto generated value column index in the current table auto generated value column collection iterated<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:INDEX::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current column index in the current table column collection iterated<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current column name from the iteration<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE(...)::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the specified language current column value type conversion (ex: Java, CSharp, ...)<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:TYPE::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current column database model type<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is the first column from the iterated auto generated value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is the last column from the iterated auto generated value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the first column from the iterated auto generated value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the last column from the iterated auto generated value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by empty value or by the inner context when the current column is the first column from the current table column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by empty value or by the inner context when the current column is the last column from the current table column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:AUTO(...)KEY:AUTO:::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current iteration column is an auto generated value column<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the first column from the iterated column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the last column from the iterated column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:AUTO(...)KEY:NOT:AUTO:::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current iteration column is not an auto generated value column<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:NULL(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current iteration column is not a nullable value column<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:PRIMARY(...)KEY:NOT:PRIMARY:::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current iteration column is not a primary key column<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:PRIMARY(...)KEY:PRIMARY:::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current iteration column is a primary key column<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the first column from the iterated auto generated value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the last column from the iterated auto generated value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the first column from the iterated not auto generated value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the last column from the iterated not auto generated value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:NULL:CURRENT:IS:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is the first column from the iterated not nullable value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:NULL:FOREACH:CURRENT:IS:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is the last column from the iterated not nullable value column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is the first column from the iterated not primary key column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is the last column from the iterated not primary key column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the first column from the iterated not primary key column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the last column from the iterated not primary key column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is the first column from the iterated primary key column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is the last column from the iterated primary key column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the first column from the iterated primary key column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(...):::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the inner context when the current column is not the last column from the iterated primary key column collection<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:INDEX::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current not auto generated value column index in the current table not auto generated value column collection iterated<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:PRIMARY:NOT:NULL:CURRENT:INDEX::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current not nullable value column index in the current table not nullable value column collection iterated<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:INDEX::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current not primary key column index in the current table not primary key column collection iterated<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:INDEX::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current primary key column index in the current table primary key column collection iterated<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:AUTO:FOREACH[...]::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the intern context as many time as there is auto generated value column in the table<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:FOREACH[...]::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the intern context as many time as there is column in the table<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH[...]::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the intern context as many time as there is not auto generated value column in the table<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:NULL:FOREACH[...]::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the intern context as many time as there is not nullable value column in the table<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH[...]::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the intern context as many time as there is not primary key column in the table<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:COLUMN:PRIMARY:FOREACH[...]::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the intern context as many time as there is primary key column in the table<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:CURRENT:NAME::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current table name<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:CURRENT:NAME::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the current database name<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:TABLE:FOREACH[...]::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the intern context as many time as there is table in the database<!--!-->
                </div><!--!-->
            </div><!--!-->
            <div class="row"><!--!-->
                <div class="col"><!--!-->
                    {:TDB:FUNCTION:FIRST:CHARACTER:TO:UPPER:CASE(...)::}<!--!-->
                </div><!--!-->
                <div class="col"><!--!-->
                    Is replaced by the intern context with the first letter of intern context Uppercased<!--!-->
                </div><!--!-->
            </div><!--!-->
    </div>