<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html"/>
	    <xsl:template match="/">
	    <HTML>
		<HEAD>
			<TITLE> Exception Interceptor - By RadicalConcepts </TITLE>
			<style type="text/css">
				.TBLAUDIT{border:#A2A2A2 1px solid;}
				.QROWS{border:#A5BED1 1px solid;}
				.CBL{border:#000000 1px solid;}
				.TABLEHEADERS{
					 border-left:#A2A2A2 1px solid;
					 border-bottom:1px outset #FFFFFF;
					 border-top:2px outset #FFFFFF;
					 border-right:1px outset #A2A2A2;
					 background:#CCCCCC;
				}
				.HEADERS{
					font-family:Tahoma;
					font-size:11px;
					color:#274E4E;
				}
				.FilesScanned
				{
					font-family:Tahoma;
					font-size:11px;
					color:#FF6600;
				}
			</style>
		</HEAD>
	    <BODY text="#000000" bgColor="#ffffff" leftMargin="0" topMargin="0" marginheight="0" marginwidth="0">

	    <table class="TBLAUDIT" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
		<tr>
			<td align="center">
				<H1><font face="Verdana" color="#0C70C0" size="3"> Exception Interceptor - By RadicalConcepts </font> </H1>
			</td>
			<td align="right">
				<font class="FilesScanned"> Tool Version 1.0 </font>
			</td>
		</tr>
	    </table>

		<xsl:apply-templates select="DAExceptionCodeAudit"/>

	    </BODY>
	    </HTML>
	</xsl:template>

	<xsl:template match="DAExceptionCodeAudit">

	    <b> <font class="FilesScanned"> Total Files Scanned : <xsl:value-of select="Projects/@TotalSourceFiles"/> </font> </b>

	    <xsl:apply-templates select="Projects"/>
	</xsl:template>

	<xsl:template match="Projects">
		<xsl:for-each select="Project">
			<table class="TBLAUDIT" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<TD width="50%"> <font class="HEADERS"> <b>Project Name</b> </font> </TD>
					<TD width="50%"> <font class="HEADERS"> <b><xsl:value-of select="@Name"/></b> </font> </TD>
				</tr>
				<tr>
					<TD width="50%"><font class="HEADERS"> <b>File Name</b> </font></TD>
					<TD width="50%"> <font class="HEADERS"> <b><xsl:apply-templates select="SourceFiles"/></b> </font> </TD>
				</tr>
				<tr>
					<TD colspan="2">
						<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
							<tr>
								<TD width="5%" bgcolor="#48718E"> <font face="Fixedsys" color="#FFFFFF" size="2"> S.No. </font> </TD>
								<TD width="20%" bgcolor="#48718E"> <font face="Fixedsys" color="#FFFFFF" size="2"> Class Name </font> </TD>
								<TD width="20%" bgcolor="#48718E"> <font face="Fixedsys" color="#FFFFFF" size="2"> Method Name </font> </TD>
								<TD width="10%" bgcolor="#48718E"> <font face="Fixedsys" color="#FFFFFF" size="2"> Line Nos </font> </TD>
								<TD width="15%" bgcolor="#48718E"> <font face="Fixedsys" color="#FFFFFF" size="2"> Types Used </font> </TD>
								<TD width="15%" bgcolor="#48718E"> <font face="Fixedsys" color="#FFFFFF" size="2"> Exceptions Used </font> </TD>
								<TD width="15%" bgcolor="#48718E"> <font face="Fixedsys" color="#FFFFFF" size="2"> Possible Exceptions </font> </TD>
							</tr>
							<xsl:apply-templates select="SourceFiles/SourceFile"/>
						</table>
					</TD>
				</tr>
			</table>
		</xsl:for-each>
	</xsl:template>


    <xsl:template match="Project">
	<xsl:value-of select="@Name"/>
    </xsl:template>

    <xsl:template match="SourceFiles">
	<xsl:value-of select="SourceFile/@Name"/>
    </xsl:template>

    <xsl:template match="SourceFile">
		<xsl:for-each select="Class/Method">
			<tr>
				<xsl:if test="position() mod 2 = 0">
					<xsl:attribute name="bgcolor">#DDDDDD</xsl:attribute>
				</xsl:if>
				<TD width="5%" valign="top"> <font face="Verdana" color="#663300" size="1"> <xsl:number level="any"/> </font> </TD>
				<TD width="20%" valign="top"> <font face="Verdana" color="#663300" size="1"> <xsl:value-of select="../@Name"/> </font> </TD>
				<TD width="20%" valign="top"> <font face="Verdana" color="#663300" size="1"> <xsl:value-of select="@Name"/> </font> </TD>
				<TD width="10%" valign="top"> <font face="Verdana" color="#663300" size="1"> <xsl:value-of select="@LineNos"/> </font> </TD>
				<TD width="15%" valign="top"> <font face="Verdana" color="#663300" size="1"> <xsl:value-of select="BCLTypesUsed"/> </font> </TD>
				<TD width="15%" valign="top"> <font face="Verdana" color="#663300" size="1"> <xsl:value-of select="ExceptionsUsed"/> </font> </TD>
				<TD width="15%" valign="top"> <font face="Verdana" color="#663300" size="1"> <xsl:value-of select="ExpectedExceptions"/> </font> </TD>
			</tr>
		</xsl:for-each>
    </xsl:template>
</xsl:stylesheet>