using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace LD5
{
    public partial class Form1 : System.Web.UI.Page
    {
        /// <summary>
        /// Shows data on the page by creating a table for each file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="excludeCity"></param>
        /// <param name="headers"></param>
        protected void ShowData<T>(List<Tuple<string, List<T>>> data, bool excludeID, params string[] headers)
        {
            foreach (var entry in data)
            {
                Table table = new Table();
                Label label = new Label();
                divData.Controls.Add(new LiteralControl("<br/>"));
                label.Text = entry.Item1;
                divData.Controls.Add(label);
                divData.Controls.Add(table);
                ShowData(entry.Item2, table, false, headers);

            }
        }
        /// <summary>
        /// Method to display a table of data on the page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="table"></param>
        /// <param name="excludeCity"></param>
        /// <param name="headers"></param>
        protected void ShowData<T>(List<T> data, Table table, bool excludeID, params string[] headers)
        {
            TableHeaderRow headerRow = new TableHeaderRow();
            foreach (string header in headers)
            {
                CreateHeaderCell(header, headerRow, HorizontalAlign.Center);
            }

            table.Rows.Add(headerRow);
            foreach (T entry in data)
            {
                string[] parts;
                TableRow row = new TableRow();

                parts = entry.ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (!excludeID)
                {
                    for (int i = 0; i < parts.Length; i++)
                    {
                        CreateCell(parts[i].Trim(), row, HorizontalAlign.Center);
                    }
                }
                else
                {
                    for (int i = 1; i < parts.Length; i++)
                    {
                        CreateCell(parts[i].Trim(), row, HorizontalAlign.Center);
                    }
                }

                table.Rows.Add(row);
            }
        }
       
        /// <summary>
        /// Method for creating a table cell
        /// </summary>
        /// <param name="text"></param>
        /// <param name="row"></param>
        /// <param name="align"></param>
        protected void CreateCell(string text, TableRow row, HorizontalAlign align)
        {
            TableCell cell = new TableCell();

            cell.Text = text;


            cell.HorizontalAlign = align;
            row.Cells.Add(cell);
        }
        /// <summary>
        /// Methdod for creating a table header cell
        /// </summary>
        /// <param name="text"></param>
        /// <param name="row"></param>
        /// <param name="align"></param>
        protected void CreateHeaderCell(string text, TableHeaderRow row, HorizontalAlign align)
        {
            TableHeaderCell cell = new TableHeaderCell();

            cell.Text = text;


            cell.HorizontalAlign = align;
            row.Cells.Add(cell);
        }
        /// <summary>
        /// Checks if a textbox isn't empty and contains only numbers and changes given label text accordingly
        /// </summary>
        /// <param name="box"></param>
        /// <param name="label"></param>
        /// <returns>true if text box contains text, false otherwise</returns>
        public bool IsNumber(TextBox box, Label label)
        {
            string text = box.Text;
            label.Text = "";
            if (text == String.Empty)
            {
                label.Visible = true;
                label.Text = "Teksto laukas negali būti tuščias.";
                return false;
            }
            foreach(char ch in text)
            {
                if(!char.IsNumber(ch))
                {
                    label.Visible = true;
                    label.Text = "Galimi simboliai tik skaičiai.";
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Method for displaying all exceptions on the page
        /// </summary>
        /// <param name="ex"></param>
        public void ShowExceptions(AggregateException ex)
        {
            foreach(Exception e in ex.InnerExceptions)
            {
                Label label = new Label();
                label.Text = e.Message;
                label.ForeColor = System.Drawing.Color.Red;
                divStart.Controls.Add(label);
                divStart.Controls.Add(new LiteralControl("<br/>"));
            }
        }
    }
}