<!DOCTYPE internal [
    <!ENTITY nbsp " ">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">
    
        <xsl:template match="coverage">
        <html>
            <head>
                <title>Code Coverage Report</title>
                <style>BODY { font-family: Trebuchet MS; font-size: 10pt; }
                   TD { font-family: Trebuchet MS; font-size: 10pt; }
                   .title { font-size: 20pt; font-weight: bold; }
                   .assembly { font-size: 14pt; }
                   .module { color: navy; font-size: 8pt; }
                   .method { color: maroon; font-size: 12pt; font-weight: bold; }
                   .subtitle { color: black; font-size: 12pt; font-weight: bold; }
                   .hdrcell  { background-color: #DDEEFF; }
                   .datacell { background-color: #FFFFEE; text-align: right; }
                   .gooddatacell {background-color: #CCFFCC; text-align: right; }
                   .hldatacell { background-color: #FFCCCC; text-align: right; }
                </style>
            </head>
            <body>
                <div class="title">Code Coverage Report</div>
                <p></p>
                <xsl:apply-templates select="module" />
            </body>
        </html>
    </xsl:template>

    <xsl:template match="module">
        <div class="assembly"><xsl:value-of select="@assembly"/></div>
        <div class="module"><xsl:value-of select="@name"/></div>
        <p></p>
        <xsl:apply-templates select="method"/>
    </xsl:template>
    
    <xsl:template match="method">
        <div class="method"><xsl:value-of select="@class"/>.<xsl:value-of select="@name"/></div>
        <table border="1" cellpadding="3" cellspacing="0" bordercolor="black">
            <tr>
                <td class="hdrcell">Visit Count</td>
                <td class="hdrcell">Line</td>
                <td class="hdrcell">Column</td>
                <td class="hdrcell">End Line</td>
                <td class="hdrcell">End Column</td>
                <td class="hdrcell">Document</td>
            </tr>
            <xsl:apply-templates select="seqpnt"/>
        </table>
        <p></p>
    </xsl:template>

    <xsl:template match="seqpnt">
        <tr>
        <td class="datacell">
            <xsl:attribute name="class">
                <xsl:choose>
                    <xsl:when test="@visitcount = 0">hldatacell</xsl:when>
                    <xsl:when test="@visitcount != 0">gooddatacell</xsl:when>
                    <xsl:otherwise>datacell</xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:value-of select="@visitcount"/>
        </td>
        <td class="datacell"><xsl:value-of select="@line"/></td>
        <td class="datacell"><xsl:value-of select="@column"/></td>
        <td class="datacell"><xsl:value-of select="@endline"/></td>
        <td class="datacell"><xsl:value-of select="@endcolumn"/></td>
        <td class="datacell"><xsl:value-of select="@document"/></td>
        </tr>
    </xsl:template>
    
</xsl:stylesheet>