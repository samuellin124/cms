<%@ Page Language="c#" CodeFile="default.aspx.cs" AutoEventWireup="false" Inherits="System.Web.UI.DataVisualization.Charting.Samples.IndexPage" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Chart for ASP.NET Samples</title>

    <script>
        var infoPagesFrameRows = "*,40%";
    </script>

</head>
<frameset rows="400,*" id="mainFrameSet" framespacing="0" frameborder="0">
		<frame noresize scrolling="no" ID="headerFrame" src="Header.htm">
		<frameset cols="256,*" ID="topFrameSet" frameSpacing="2" frameBorder="0">
			<frame scrolling="no" ID="navigationFrame" src="NavigationFrameset.htm">
			<frame scrolling="no" ID="sampleContentFrame" src="SampleContentFrameset.htm">
		</frameset>
		<noframes>
			<body>
				<p>This page uses frames, but your browser doesn't support them.</p>
			</body>
		</noframes>
	</frameset>
</html>
