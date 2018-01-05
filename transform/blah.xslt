<?xml version="1.0" encoding="utf-8"?>


<xsl:stylesheet
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    
    <xsl:output media-type="text" omit-xml-declaration="yes"/>
    <xsl:param name="Boss_ID" select="'invalid'"/>

    <xsl:template match="/package/metadata/id" mode="batRemark">
        <xsl:value-of select ="concat('set ID=',text(),'&#x0d;')"/>
    </xsl:template>
    
    <xsl:template match="/package/metadata/version" mode="batRemark">
        <xsl:value-of select ="concat('set VERSION=',text(),'&#x0d;')"/>
    </xsl:template>

    <xsl:template match="text()"/>
    <xsl:template match="text()" mode="batRemark"/>

    <xsl:template match="/">
        <xsl:text>@echo off&#x0d;</xsl:text>
        <xsl:value-of select="concat('REM Boss_ID=','$Boss_ID=')"/>
        <xsl:apply-templates mode="batRemark"/>
    </xsl:template>

</xsl:stylesheet>
