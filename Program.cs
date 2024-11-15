using CS_4308_Term_Project;

class Program
{
    public static void Main(System.String[] args)
    {
        try
        {
            List<Lexical> tokens = Scanner.scan(args[0]);
            //calls the scan function with with the file name given as an argument
            Node root = new Node(null);
            root.body = tokens;
            //Creates the root Node
            Console.WriteLine("Program:");
            Parser.Parse(root);
            //Calls parse method with the root node
            Node.printParseTree(root, 1);
            //Calls the static Node.printParseTree method with the root node
            Console.WriteLine("\nParse Successful");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine ("File Could Not Be Located");
        }
        catch (SyntaxError error)
        {
            Console.WriteLine(error.OutPut);
        }
        finally
        {
            Console.ReadLine();
        }
    }
}