using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace HelloForms
{
    public class MyDataObject : IDataObject
    {
        System.Collections.Hashtable _Data = new System.Collections.Hashtable();

        public MyDataObject() { }

        public MyDataObject(object data)
        {
            SetData(data);
        }

        public MyDataObject(string format, object data)
        {
            SetData(format, data);
        }

        #region IDataObject Members

        public object GetData(Type format)
        {
            return _Data[format.FullName];
        }

        public object GetData(string format)
        {
            return _Data[format];
        }

        public object GetData(string format, bool autoConvert)
        {
            return GetData(format);
        }

        public bool GetDataPresent(string format)
        {
            return _Data.ContainsKey(format);
        }

        public bool GetDataPresent(string format, bool autoConvert)
        {
            return GetDataPresent(format);
        }

        public bool GetDataPresent(Type format)
        {
            return _Data.ContainsKey(format.FullName);
        }

        public string[] GetFormats()
        {
            string[] strArray = new string[_Data.Keys.Count];
            _Data.Keys.CopyTo(strArray, 0);
            return strArray;
        }

        public string[] GetFormats(bool autoConvert)
        {
            return GetFormats();
        }

        public void SetData(Type format, object data)
        {
            SetData(data, format.FullName);
        }

        public void SetData(string format, bool autoConvert, object data)
        {
            SetData(data, format);
        }

        public void SetData(string format, object data)
        {
            SetData(data, format);
        }

        private void SetData(object data, string format)
        {
            object obj = new DataContainer(data);

            if (string.IsNullOrEmpty(format))
            {
                // Create a dummy DataObject object to retrieve all possible formats.
                // Ex.: For a System.String type, GetFormats returns 3 formats:
                // "System.String", "UnicodeText" and "Text"
                System.Windows.Forms.DataObject dataObject = new System.Windows.Forms.DataObject(data);
                foreach (string fmt in dataObject.GetFormats())
                {
                    _Data[fmt] = obj;
                }
            }
            else
            {
                _Data[format] = obj;
            }
        }

        public void SetData(object data)
        {
            SetData(data, null);
        }

        #endregion
    }
}
