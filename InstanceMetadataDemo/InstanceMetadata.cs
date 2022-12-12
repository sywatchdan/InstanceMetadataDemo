using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.EC2;
using Amazon.EC2.Internal;
using Amazon.EC2.Model;
using Amazon.Util;
using Amazon.Runtime.Internal.Transform;

namespace InstanceMetadataDemo {
    internal class InstanceMetadata {
        public InstanceMetadata() { }

        public string InspectMetadataItems(string startingPath) {

            Dictionary<string, object> values = new Dictionary<string, object>();

            // Get the name for the first object. This will either be the requested key, or 'Metadata' if no key was specified
            string startingPathName = (startingPath == "/" ? "Metadata" : startingPath.Substring(startingPath.LastIndexOf("/") + 1));

            // AWS SDK BUG FIX 1: Requested path needs to end with a trailing slash
            if (startingPath.EndsWith("/") == false) { startingPath += "/"; }

            // AWS SDK BUG FIX 2: Calling GetItems on a key value pair incorrectly returns the value as a child key. To get around this, we check
            // that the value is equal to what it thinks is a child key, and that querying that child key returns null.
            IEnumerable<string> metadataItems = EC2InstanceMetadata.GetItems(startingPath);

            try {
                if (metadataItems.Count() == 1 && metadataItems.First() == EC2InstanceMetadata.GetData(startingPath) && EC2InstanceMetadata.GetData(startingPath + metadataItems.First()) is null) {
                    // This is just a key value pair. Add it to our dictionary
                    values.Add(startingPathName, EC2InstanceMetadata.GetData(startingPath));

                } else {

                    // Not affected by the bug. Recurse through child keys
                    values.Add(startingPathName, RecurseItems(startingPath));
                }
            } catch(Exception ex) {
                if(ex is System.ArgumentNullException) {
                    Console.WriteLine("The requested path was not found in the Metadata");
                } else {
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                }
            }

            return (JsonConvert.SerializeObject(values));
        }

        private Dictionary<string, object> RecurseItems(string path) {

            Dictionary<string, object> values = new Dictionary<string, object>();
            
            // Get all items (properties) in this key and loop through them
            IEnumerable<string> metadataItems = EC2InstanceMetadata.GetItems(path);
 
            foreach (string propertyName in metadataItems) {

                // If the property name ends with a trailing slash, it has children to recurse through, otherwise it contains just a single value
                if(propertyName.EndsWith("/")) {
                    values.Add(propertyName.Remove(propertyName.Length - 1), RecurseItems(path + propertyName));

                } else {
                    string propertyData = EC2InstanceMetadata.GetData(path + propertyName);
                    values.Add(propertyName, propertyData);
                }
            }
            return values;
        }
        
    }
}
