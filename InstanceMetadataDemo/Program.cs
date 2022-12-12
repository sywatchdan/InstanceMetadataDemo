using InstanceMetadataDemo;

Console.WriteLine("InstanceMetadataDemo");
Console.WriteLine("This demo app will enumerate items in the EC2 Instance Metadata and output them as JSON");
Console.WriteLine();
Console.Write("Enter a specific value to search for or leave blank for all: ");

string startingPath = "/" + Console.ReadLine();

// Spin up a new InstanceMetadata class and get it to produce the output
InstanceMetadata instanceMetadata = new InstanceMetadata();
string outputJson = instanceMetadata.InspectMetadataItems(startingPath);

// Print the response
Console.WriteLine(outputJson);

