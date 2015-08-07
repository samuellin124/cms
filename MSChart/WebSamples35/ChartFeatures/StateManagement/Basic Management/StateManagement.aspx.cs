using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.DataVisualization.Charting;

namespace System.Web.UI.DataVisualization.Charting.Samples
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class StateManagement : System.Web.UI.Page
	{

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			
			// check to see if there is a session variable created that
			// retains the previous page state.  If a user deselects the 
			// state management and then selects it, the data will need to
			// be reloaded.  This is another flag to indicate the need for data.
			string enabled = "";
			try
			{
				enabled = this.Session["EnableStateManagement"].ToString();
			}
			catch(Exception )
			{
				enabled = EnableStateManagement.Checked.ToString();
			}

			// save the current selection state to a session variable.
			this.Session["EnableStateManagement"] = EnableStateManagement.Checked.ToString();

			// determine what content should be serialized in the browser.
			Chart1.ViewStateContent = SerializationContents.Default;

			// if this is not a postback or if state management is not selected, then
			// add the source chart data to the chart.  Also, if there was no state
			// management in the previous page view and there is now, reload the data
			if(!EnableStateManagement.Checked || !IsPostBack ||
				(enabled=="False" && EnableStateManagement.Checked) )
			{
				// Generate random data.  
				Data( Chart1.Series["Input"] );
			}

			Chart1.Series["Input"].ChartType = SeriesChartType.Line;

			// Add the other series to the chart
			AddOtherSeries();
			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Init += new System.EventHandler(this.Form_Init);

		}
		#endregion

		
		

		/// <summary>
		/// This method adds additional series based on a the state of the web controls.  These
		/// series will not be persisted therefore their state will not be managed.
		/// </summary>
		private void AddOtherSeries()
		{

			// remove the Above series to avoid a duplication from one that was serialized and 
			// one that we may create now
			int index = Chart1.Series.IndexOf("Above");
			if(index >= 0)
				Chart1.Series.RemoveAt(index);

			// remove the Below series to avoid a duplication from one that was serialized and 
			// one that we may create now
            index = Chart1.Series.IndexOf("Below");
			if(index >= 0)
				Chart1.Series.RemoveAt(index);

			// Adds a duplicate series of the source shifted up by 5
			if(SeriesAbove.Checked)
			{
				Chart1.Series.Add("Above");
				Chart1.Series["Above"].ChartType = SeriesChartType.Line;
				foreach (DataPoint pt in Chart1.Series["Input"].Points)
				{
					Chart1.Series["Above"].Points.AddXY(pt.XValue,pt.YValues[0]+5);
				}
			}

			// Adds a duplicate series of the source shifted down by 5
			if(SeriesBelow.Checked)
			{
				Chart1.Series.Add("Below");
				Chart1.Series["Below"].ChartType = SeriesChartType.Line;
				foreach (DataPoint pt in Chart1.Series["Input"].Points)
				{
					Chart1.Series["Below"].Points.AddXY(pt.XValue,pt.YValues[0]-5);
				}
			}
		}

		/// <summary>
		/// This method generates random data.
		/// </summary>
		/// <param name="series"></param>
		private void Data( Series series )
		{
			// clear any and all points that may exist in the points collection
			// so that they don't become appended to a previous set of data that
			// may have been serialized in another page load.
			series.Points.Clear();

			Random rand;
			// Use a number to calculate a starting value for 
			// the pseudo-random number sequence
			rand = new Random(100);

			// Generate 50 random y values.
			for( int index = 0; index < 25; index++ )
			{
				// Generate the first point
				series.Points.AddXY(index+1,0);
				series.Points[index].YValues[0] = 10;

				// Use previous point to calculate a next one.
				if( index > 0 )
					series.Points[index].YValues[0] = series.Points[index-1].YValues[0] + 4*rand.NextDouble() - 2;

			}
		}

		protected void LoadData_Click(object sender, System.EventArgs e)
		{
			// do nothing.... the Page_Load will be called and will
			// do all the work required.
		}

		protected void Form_Init(object sender, System.EventArgs e)
		{
			// enable or disable the view state
			Chart1.EnableViewState = EnableStateManagement.Checked;
		}
	}
}
