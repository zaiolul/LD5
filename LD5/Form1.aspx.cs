using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace LD5
{
    public partial class Form1 : System.Web.UI.Page
    {
        private string resultsFile;
        private List<Tuple<string, List<Valuable>>> valuables;
        private List<Order> orderItems;
        private List<Valuable> mergedValuables;
        private List<Valuable> cheapestValuables;
        protected void Page_Load(object sender, EventArgs e)
        {
            resultsFile = Server.MapPath("/App_Data/Rez.txt");

            if (Page.IsPostBack)
            {   
                //---------GET DATA AFTER PAGE LOAD-------------
                mergedValuables = Session["merged"] as List<Valuable>;
                cheapestValuables = Session["cheapest"] as List<Valuable>;
                orderItems = Session["order"] as List<Order>;
                valuables = Session["valuables"] as List<Tuple<string, List<Valuable>>>;   
                //----------------------------------------------
            }
            else
            {
                divData.Visible = false;
                divResults.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(resultsFile))
            {
                File.Delete(resultsFile);
            }
            try
            {   
                //------------READ DATA-----------------------------------------------------
                string dataDir = Server.MapPath("App_Data");
                string[] storagePaths = Directory.GetFiles(dataDir, "Storage*.txt");
                string[] orderPath = Directory.GetFiles(dataDir, "Order.txt");

                if (valuables == null)
                {
                    valuables = InOut.ReadValuables(storagePaths);
                }
                if (orderItems == null)
                {
                    orderItems = InOut.ReadOrder(orderPath[0]);
                }
                //--------------------------------------------------------------------
                mergedValuables = valuables.SelectMany(entry => entry.Item2).Distinct().ToList(); //merges all data into one list

                cheapestValuables = mergedValuables.
                    Where(val => val.Price == mergedValuables.Min(v => v.Price)).ToList(); //finds all cheapest valuables
                //-------------SHOW DATA ON THE PAGE----------------------------------
                ShowData(valuables, false, "Sandėlio nr.", "Pavadinimas", "Kiekis", "Kaina");
                var temp = new List<Tuple<string, List<Order>>>();
                temp.Add(new Tuple<string, List<Order>>("Order.txt", orderItems));
                ShowData(temp, false, "Pavadinimas", "Kiekis");
                ShowData(cheapestValuables, Table1, false, "Sandėlio nr.", "Pavadinimas", "Kiekis", "Kaina");
                //-------------------------------------------------------------------

                //-------------PRINT DATA TO FILE------------------------------------
                InOut.PrintToTxtFile(valuables, resultsFile, "",
                    String.Format("| {0,-13} | {1,-11} | {2,-8} | {3,-5} |", "Sandėlio nr.", "Pavadinimas", "Kiekis", "Kaina"));
                InOut.PrintToTxtFile(temp, resultsFile, "",
                    String.Format("| {0,-11} | {1, -8} |", "Pavadinimas", "Kiekis"));
                InOut.PrintToTxtFile(cheapestValuables, resultsFile, "Pigiausios vertybės:",
                    String.Format("| {0,-13} | {1,-11} | {2,-8} | {3,-5} |", "Sandėlio nr.", "Pavadinimas", "Kiekis", "Kaina"));
                //-------------------------------------------------------------------

                //------------SAVE DATA BETWEEN PAGE LOADS---------------------
                Session["valuables"] = valuables;
                Session["order"] = orderItems;
                Session["merged"] = mergedValuables;
                Session["cheapest"] = cheapestValuables;
                Session["order"] = orderItems;
                //-------------------------------------------------------------

                divData.Visible = true;
                divResults.Visible = true;
            }
            catch (AggregateException ex)
            {
                ShowExceptions(ex);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //-------------SHOW DATA ON THE PAGE----------------------------------
            ShowData(valuables, false, "Sandėlio nr.", "Pavadinimas", "Kiekis", "Kaina");
            var temp = new List<Tuple<string, List<Order>>>();
            temp.Add(new Tuple<string, List<Order>>("Order.txt", orderItems));
            ShowData(temp, false, "Pavadinimas", "Kiekis");
            ShowData(cheapestValuables, Table1, false, "Sandėlio nr.", "Pavadinimas", "Kiekis", "Kaina");
            //--------------------------------------------------------------------
            if (IsNumber(TextBox1, Label1))
            {

                List<Valuable> filterOrder = mergedValuables.Where(val =>
                orderItems.Any(ord => val.Name == ord.Name))
                .OrderBy(val1 => val1.Price).ToList(); //gets all items that are in the order, ordered by price

                var sum = filterOrder.Sum(val => val.Price * val.Count); // finds the sum of all valuables
                List<Valuable> filterOrderRemoved = filterOrder.SkipWhile(val =>
                {
                    if (sum > int.Parse(TextBox1.Text))
                    {
                        sum -= val.Price * val.Count;
                        return true;
                    }
                    return false;
                }
                ).OrderBy(val1 => val1.Name).ThenBy(val2 => val2.Count).ToList(); // removes all cheapest elements while sum of all is greater than given value
                //orders by name and item count

                //-----------------DISPLAY AND PRINT DATA----------------------
                if(filterOrderRemoved.Count > 0)
                {
                    Label2.Text = "Pirkėjui atrinktos vertybės:";
                    ShowData(filterOrderRemoved, Table2, false, "Sandėlio nr.", "Pavadinimas", "Kiekis", "Kaina");
                    InOut.PrintToTxtFile(filterOrderRemoved, resultsFile, String.Format("Atrinktų vertybių suma: {0}\nPirkėjui atrinktos vertybės:", sum),
                        String.Format("| {0,-13} | {1,-10} | {2,-8} | {3,-5} |", "Sandėlio nr.", "Pavadinimas", "Kiekis", "Kaina"));
                    Label3.Text = String.Format("Atrinktų vertybių suma: {0}", sum);
                    Label3.Visible = true;
                }
                else
                {
                    Label2.Text = "Nėra pirkėjui tinkamų vertybių.";
                    InOut.PrintToTxtFile(new List<Valuable>(), resultsFile, "Nėra pirkėjui tinkamų vertybių.", "");
                }
                //---------------------------------------------------------------------
                
                Table2.Visible = true;
                Label1.Visible = true;
                Label2.Visible = true;
                
            }
        }
    }
}