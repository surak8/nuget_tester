<xsl:stylesheet
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:output
        media-type="xml"
        omit-xml-declaration="yes"
        indent="yes" />
    
    <xsl:param name="Boss_ID" select="'ERROR'"/>

    <xsl:template match="/">
        <xsl:element name="blah">
            <xsl:comment>
                <xsl:value-of select="concat('dollar-version: ',$Boss_ID)"/>
            </xsl:comment>
        </xsl:element >
    </xsl:template>
</xsl:stylesheet>