<?xml version="1.0" encoding="utf-8"?>


<xsl:stylesheet
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:output
     media-type="xml"
     omit-xml-declaration="no"
     indent="yes" />

    <!--<xsl:variable name="vOwners" select="'none'"/>-->
    <xsl:template match="package">
        <xsl:element name="{local-name()}">
            <xsl:apply-templates/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="metadata">
       <xsl:param name="vOwners"/>
           <xsl:element name="{local-name()}">
            <xsl:copy-of select="./id"/>
            <xsl:copy-of select="./version"/>
            <xsl:copy-of select="./description"/>
            <xsl:copy-of select="./authors"/>
            <xsl:element name="owners">
                <xsl:value-of select ="$vOwners"/>
            </xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template match="text()"/>
</xsl:stylesheet>