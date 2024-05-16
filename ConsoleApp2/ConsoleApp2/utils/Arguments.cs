using CommandLine;

namespace ConsoleApp2.utils
{
    record class Arguments
    {
        [Option('o',"output",Required = true, HelpText = "Path on the disk where to save the cat pic")]
        public string OutputPath { get; set; }
        [Option('t', "catSaysText", Required = false, HelpText = "What Should The Cat Say")]
        public string CatSaysText { get; set; }
    }
}
