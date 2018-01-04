<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet    version="1.0"    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output          omit-xml-declaration="yes"     indent="yes" encoding="us-ascii"   method="xml" />
    <xsl:param name="vOwners"/>

    <xsl:template match ="/">
        <xsl:apply-templates>
            <xsl:with-param name="vOwners" select="$vOwners"/>
        </xsl:apply-templates>
    </xsl:template>

    <xsl:template match="package">
        <xsl:param name="vOwners"/>
        <xsl:element name="{local-name()}">
            <xsl:apply-templates>
                <xsl:with-param name="vOwners" select="$vOwners"/>
            </xsl:apply-templates>
        </xsl:element>
    </xsl:template>

    <xsl:template match="metadata">
        <xsl:param name="vOwners"/>
        <xsl:element name="{local-name()}">
            <xsl:copy-of select="./id"/>
            <xsl:copy-of select="./version"/>
            <xsl:copy-of select="./description"/>

            <xsl:if test="$vOwners!=''">
                <xsl:element name="owners">
                    <xsl:value-of select ="$vOwners"/>
                </xsl:element>
            </xsl:if>

            <!--<xsl:for-each select="./*">
                <xsl:comment >
                    <xsl:value-of select="name(.)"/>
                </xsl:comment>
            </xsl:for-each>-->
        </xsl:element>
    </xsl:template>

    <xsl:template match="node()">
        <xsl:comment>
            <xsl:text>blah</xsl:text>
        </xsl:comment>
    </xsl:template>
    <xsl:template match="text()"/>
</xsl:stylesheet>