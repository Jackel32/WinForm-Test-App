using System;
using System.Diagnostics;
namespace MVCTest
{
    public delegate void ModelHandler<IModel>(IModel sender, ModelEventArgs e);
    // The ModelEventArgs class which is derived from th EventArgs class to 
    // be passed on to the controller when the value is changed
    public class ModelEventArgs : EventArgs
    {
        public string newval;
        public ModelEventArgs(string t)
        {
            newval = t;
        }
    }
    // The interface which the form/view must implement so that, the event will be
    // fired when a value is changed in the model.
    public interface IModelObserver
    {
        void valueChanged(IModel model, ModelEventArgs e);
    }
    // The Model interface where we can attach the function to be notified when value
    // is changed. An actual data manipulation function increment which increments the value
    // A setvalue function which sets the value when users changes the textbox
    public interface IModel
    {
        void attach(IModelObserver imo);
        void update();
        void setvalue(string t);
    }
    public class IncModel : IModel
    {
        public event ModelHandler<IncModel> changed;
        string text;
        // implementation of IModel interface set the initial value to 0
        public IncModel()
        {
            text = "";
        }
        // Set value function to set the value in case if the user directly changes the value
        // in the textbox and the view change event fires in the controller
        public void setvalue(string t)
        {
            text = t;
        }
        // Change the value and fire the event with the new value inside ModelEventArgs
        // This will invoke the function valueIncremented in the model and will be displayed
        // in the textbox subsequently
        public void update()
        {
            text = run_cmd();
            changed.Invoke(this, new ModelEventArgs(text));
        }
        // Attach the function which is implementing the IModelObserver so that it can be
        // notified when a value is changed
        public void attach(IModelObserver imo)
        {
            changed += new ModelHandler<IncModel>(imo.valueChanged);
        }

        private string run_cmd()
        {

            string fileName = @"C:\Users\Matt\Documents\GitHub\StockAnalysis\stock.py";

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@"C:\Python27\python.exe", fileName)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            //Console.WriteLine(output);

            //Console.ReadLine();
            return output;
        }

    }
}