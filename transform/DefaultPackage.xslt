<xsl:stylesheet
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:output
        media-type="xml"
        omit-xml-declaration="yes"
        indent="yes" />

    <xsl:variable name="apos">'</xsl:variable>
    <xsl:param name="idValue" select="concat($apos,'idValue',$apos,' not set.')"/>
    <xsl:param name="descValue" select="concat($apos,'descValue',$apos,' not set.')"/>
    <xsl:param name="versionValue" select="concat($apos,'versionValue',$apos,' not set.')"/>
    <xsl:param name="authorsValue" select="concat($apos,'authorsValue',$apos,' not set.')"/>
    <!--<package>
    <metadata>
        <id>someid</id>
        <description>somedesc</description>
        <version>1.0.0</version>
        <authors>rik</authors>
    </metadata>
</package>-->
    <xsl:template match="/">
        <xsl:element name="package">
            <xsl:element name="metadata">
                <xsl:element name="id">
                    <xsl:value-of select ="$idValue"/>
                </xsl:element>
                <xsl:element name="description">
                    <xsl:value-of select ="$descValue"/>
                </xsl:element>
                <xsl:element name="version">
                    <xsl:value-of select ="$versionValue"/>
                </xsl:element>
                <xsl:element name="authors">
                    <xsl:value-of select ="$authorsValue"/>
                </xsl:element>
            </xsl:element>
        </xsl:element>
    </xsl:template>
</xsl:stylesheet>