using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
//using System.IO.StreamWriter;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public struct instruction
        {
           
            public string InstructionName;
            public char format;
            public int op;
            public int funct;
        }
        public instruction fill_inst(string sInstructionName, char sformat, int sop, int sfunct)
        {
            instruction i;
            i.InstructionName = sInstructionName;
            i.format = sformat;
            i.op = sop;
                i.funct = sfunct;
                return i;
        }
        public string RemoveSpacesFromFrontAndBack(string a)
        { 
            char[] holder = new char[1111];
            
            int j = 0,start=0;
            for (int i=0;i<a.Length;i++)
            {
                if (a[i] != ' ')
                {
                    holder[j] = a[i];
                    j++;
                    start = i;
                    break;
                }
            }
            int jj = 0;
            for (int i = a.Length-1; i>j; i--)
            {
                if (a[i] != ' ')
                {
                    jj = i;
                    break;
                }
            }
            for (int i = start+1; i <= jj; i++)
            {
                if ((i > 1) && (a[i] == ' ') && (a[i - 1] == ' '))//we will leave it 
                    j = j + 0;
                else
                {
                    holder[j] = a[i];
                    j++;
                }
                 
            }
            string myString = new string('*', j);
            StringBuilder sb = new StringBuilder(myString);
        
            for (int i = 0; i < j; i++)
                   sb[i]= holder[i] ;
               
            return sb.ToString();
        }
        public string RemoveCommentSection(string a)
        {
            char[] holder = new char[1111];

            int j = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != '#')
                {
                    holder[j] = a[i];
                    j++;
                }
                else
                    break;
            }
            string myString = new string('*', j);
            StringBuilder sb = new StringBuilder(myString);

            for (int i = 0; i < j; i++)
                sb[i] = holder[i];

            return sb.ToString();
        }
        public bool WriteToFile(string[] memory, int sizee,bool data)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Somaya\Desktop\w\WriteLines2.txt"))
            {
                for (int i = 0; i < sizee;i++)
                { 
                        file.WriteLine("MEMORY("+i.ToString()+ ") <= "+memory[i]+ " ;" );
                    
                }
            }


            return true;
        }
        public bool WriteToFileText(string[] memory, int sizee, bool data)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Somaya\Desktop\w\WriteLines3.txt"))
            {
                for (int i = 0; i < sizee; i++)
                {
                    file.WriteLine("MEMORY(" + i.ToString() + ") := " + '"' + memory[i] + '"' + " ;");

                }
            }


            return true;
        }
        public Form1()
        {
            InitializeComponent();
          
        }
        public string RegisterConverter(string Reg)
        {
            string ret = "";
            int holder = 0;
            // "$zero" 
            Regex Zero_Register_numbers = new Regex("^[$]zero$");//add 0
            Regex First_Register_numbers = new Regex("^[$]t[0-7]$");//add 8
            Regex Second_Register_numbers = new Regex("^[$]t[8-9]$");//add 16
            Regex Third_Register_numbers = new Regex("^[$]s[0-7]$");//add 16
            if (Zero_Register_numbers.IsMatch(Reg))
                return   "00000";
            else if (First_Register_numbers.IsMatch(Reg))
                {
                    Int32.TryParse(Reg.Substring(2,1), out holder);
                    holder = holder + 8;
                }
            else if (Second_Register_numbers.IsMatch(Reg))
            {
                Int32.TryParse(Reg.Substring(2, 1), out holder);
                holder = holder + 16;
            }
            else if (Third_Register_numbers.IsMatch(Reg))
            {
                Int32.TryParse(Reg.Substring(2, 1), out holder);
                holder = holder + 16;
            }

              
                ret = Convert.ToString(holder, 2);
                return ret;
         }
        public string GatherAround(string Fifthpart, string SecPart, string ThirPart, string FourPart, string SixPart)
        {
            string final = "00000000000000000000000000000000";


            final = final.Insert(11 - SecPart.Length, SecPart);
            final = final.Insert(16 - ThirPart.Length, ThirPart);
            final = final.Insert(21 - FourPart.Length, FourPart);
            final = final.Insert(32 - SixPart.Length, SixPart);
            final = final.Substring(0, 32);
            
            return final;
        }
        public int DataLineWORK(string DataLine, string[] memory, int start, string[] DatamemoryNames)
        {
            string nameHolder="";
            int size = 0, valuescounter = 0;
            int[] values = new int[11000];
            bool array = false, oneWord = false;
            // string value = "arr1: .word 3, 2, 4";
            char[] delimiters = new char[] { ' ', ',',':' };
            string[] parts = DataLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                if (i == 0)
                   nameHolder = parts[i];
               // MessageBox.Show(parts[i]);
                if (array)
                { Int32.TryParse(parts[i], out size); break; }
                else if (oneWord)
                {
                   
                        Int32.TryParse(parts[i], out values[valuescounter]); 
                    
                    valuescounter++;
                }
                if (parts[i] == ".space")
                { array = true;  }
                else if (parts[i] == ".word")
                    oneWord = true;

            }
            
            /*
            string[] words = DataLine.Split(' ');
           
            foreach (string word in words)
            {      MessageBox.Show("WORD: " + word);
                if (array)
                { Int32.TryParse(word, out size); break; }
                else if (oneWord)
                {
                    if (word[word.Length-1] == ',')
                        Int32.TryParse(word.Substring(0, word.Length-1), out values[valuescounter]); 
                    else
                    Int32.TryParse(word, out values[valuescounter]); 
                    
                    valuescounter++;
                }
                if (word == ".space")
                { array = true;  }
                else if (word == ".word")
                    oneWord = true;
            }
            */
            //MessageBox.Show(size.ToString());
            int end = size + start;
            if (array)
            {
                for (int i = start; size >= 0; i++)
                {
                    DatamemoryNames[i] = nameHolder;
                    memory[i] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
                    size--;
                }
            }
            else if (oneWord)
            {
                end = valuescounter + start;
                for (int i = 0; i < valuescounter; i++)
                {
                   // MessageBox.Show("HMMM "+values[i].ToString());
                    string binary = Convert.ToString(values[i], 2);
                    DatamemoryNames[start] = nameHolder;
                    memory[start] = binary;//we need to fill the rest of the number ???????????????
                    start++;
                }
            
            }
            WriteToFile(memory, 10, true);
            return end;
        }
        public string theRinstruction(string Line, string[] Datamemory, int endDatamemory, string[] DatamemoryNames, string[] texttmemory, Dictionary<string, instruction> TheInstructions)
        {  string FirstAndFifthpart="00000",SecPart="",ThirPart="",FourPart="",SixPart="";
            char[] delimiters = new char[] { ' ', ',' };
            int value = 0;
            string[] args = Line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
           // for (int i = 0; i < args.Length; i++)
            bool found = false;
            if (args[2][0] == '$')
                SecPart = RegisterConverter(args[2]);
            else
               {
                   for (int i = 0; i < endDatamemory; i++)
                   {
                       if (DatamemoryNames[i] == args[2])
                       {
                          SecPart= Datamemory[i];
                          found = true;
                          break;
                       }
                   }
                   if (!found)
                   {
                       Int32.TryParse(args[2], out value);
                       SecPart = Convert.ToString(value, 2);
                   }
               
               }
            if (args[3][0] == '$')
                ThirPart = RegisterConverter(args[3]);
            else
            {
                found = false;
                for (int i = 0; i < endDatamemory; i++)
                {
                    if (DatamemoryNames[i] == args[3])
                    {
                        ThirPart = Datamemory[i];
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Int32.TryParse(args[3], out value);
                    ThirPart = Convert.ToString(value, 2);
                }

            }
            if (args[1][0] == '$')
                FourPart = RegisterConverter(args[1]);
            else
            {
                found = false;
                for (int i = 0; i < endDatamemory; i++)
                {
                    if (DatamemoryNames[i] == args[1])
                    {
                        FourPart = Datamemory[i];
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Int32.TryParse(args[1], out value);
                    FourPart = Convert.ToString(value, 2);
                }

            }

            foreach(var item in TheInstructions)
                    {
                        if (item.Key == args[0])
                        { 
                            SixPart = Convert.ToString(TheInstructions[item.Key].funct, 2);
                            break;
                        } 
                    }

            if (args[0] != "slt")
                return GatherAround(FirstAndFifthpart, SecPart, ThirPart, FourPart, SixPart);
            else
            {
                return GatherAround("1", SecPart, ThirPart, FourPart, SixPart);
            
            }
        }
        public string theIinstruction(string Line, string[] Datamemory, int endDatamemory, string[] DatamemoryNames, string[] texttmemory, Dictionary<string, instruction> TheInstructions)
        {
            string FirstPart = "00000", SecPart = "", ThirPart = "", FourPart = "" ;
            char[] delimiters = new char[] { ' ', ',', '(', ')' };
            int value = 0;
            string[] args = Line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
           // for (int i = 0; i < args.Length; i++)
               // MessageBox.Show("{"+args[i]+"}");
           
            foreach (var item in TheInstructions)
            {
                
                if (item.Key == args[0])
                {
                    FirstPart = Convert.ToString(TheInstructions[item.Key].op, 2);
                    break;
                }
               
            }

            bool found = false;
            if (args[3][0] == '$')
                SecPart = RegisterConverter(args[3]);
            else
            {
                for (int i = 0; i < endDatamemory; i++)
                {
                    if (DatamemoryNames[i] == args[3])
                    {
                        SecPart = Convert.ToString(i*4, 2);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Int32.TryParse(args[3], out value);
                    SecPart = Convert.ToString(value, 2);
                }

            }
            if (args[1][0] == '$')
                ThirPart = RegisterConverter(args[1]);
            else
            {
                found = false;
                 
                for (int i = 0; i < endDatamemory; i++)
                {
                    if (DatamemoryNames[i] == args[1])
                    {
                        ThirPart = Convert.ToString(i * 4, 2);// Datamemory[i];
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Int32.TryParse(args[1], out value);
                    ThirPart = Convert.ToString(value, 2);
                }

            }
            if (args[2][0] == '$')
                FourPart = RegisterConverter(args[2]);
            else
            {
                found = false;
                for (int i = 0; i < endDatamemory; i++)
                {
                   // MessageBox.Show("DatamemoryNames  : " + DatamemoryNames[i] + "args  : " + args[2]);
                    if (DatamemoryNames[i] == args[2])
                    {
                        FourPart = Convert.ToString(i * 4, 2);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Int32.TryParse(args[2], out value);
                    FourPart = Convert.ToString(value, 2);
                    
                }

            }

            string final = "00000000000000000000000000000000";
          
            final = final.Insert(6 - FirstPart.Length, FirstPart);
            final = final.Insert(11 - SecPart.Length, SecPart);
            final = final.Insert(16 - ThirPart.Length, ThirPart);
            final = final.Insert(32 - FourPart.Length, FourPart);
            final = final.Substring(0, 32);
          
            return final;
            
        }
         
         public string theJinstruction(string LabelName, string[] LabelsNames, int[] LabelsValues,int LabelCounter)
         {string theResult = "00001000000000000000000000000000" ,holderbox="";
             for (int i = 0; i < LabelCounter; i++)
             {
                 if (LabelsNames[i] == LabelName)
                 {
                     holderbox = Convert.ToString(LabelsValues[i], 2);

                     break;
                 }
             }
             theResult = theResult.Insert(32 - holderbox.Length, holderbox);
             return theResult;
         }
        private int ToInt32(string word)
        {
            throw new NotImplementedException();
        }
        private void button1_Click(object sender, EventArgs e)// corresponding machine code
        { 
            Dictionary<string, instruction> TheInstructions = new Dictionary<string, instruction>();
            TheInstructions["add"] = fill_inst("add", 'R', 0, 32);
            TheInstructions["and"] = fill_inst("and", 'R', 0, 36);
            TheInstructions["sub"] = fill_inst("sub", 'R', 0, 34);
            TheInstructions["nor"] = fill_inst("nor", 'R', 0, 39);
            TheInstructions["or"] = fill_inst("or", 'R', 0, 37);
            TheInstructions["slt"] = fill_inst("slt", 'R', 0, 42);
            TheInstructions["addi"] = fill_inst("addi", 'I', 8, -1);
            TheInstructions["lw"] = fill_inst("lw", 'I', 35, -1);
            TheInstructions["sw"] = fill_inst("sw", 'I', 43, -1);
            TheInstructions["beq"] = fill_inst("beq", 'I', 4, -1);
            TheInstructions["bne"] = fill_inst("bne", 'I', 5, -1);
            TheInstructions["j"] = fill_inst("j", 'J', 2, -1); 
            /////////////////////////////////////////////////////////////////////////////////////
            string[] Datamemory = new string[11000];
            string[] jmemory = new string[11000];
            int[] jmemorynames = new int[11000];
        string[] DatamemoryNames = new string[11000];
        string[] LabelsNames = new string[11000];
        int[] LabelsValues = new int[11000];
        string[] Textmemory = new string[11000];
        int BeginData = 0, BeginText = 0, LabelCounter = 0, jmemorycounter=0;
        char[] delimiters = new char[] { '\n' };//To split lines
        string[] parts = textBox1.Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        
        for (int di = 0; di < parts.Length; di++)
        {
           // MessageBox.Show("Line number : " + di.ToString() + "   " + parts[di]);
            string TestOneLine = parts[di];
            TestOneLine = RemoveCommentSection(TestOneLine);
           TestOneLine = RemoveSpacesFromFrontAndBack(TestOneLine);
            Regex DataLine = new Regex("^[a-zA-Z0-9]{1,50}: .[a-zA-Z0-9]{1,50} [0-9]{1,50}");
            Regex CommentLine = new Regex("^#");
            Regex jInstructionLine = new Regex("^j");
            Regex LoopLine = new Regex("^loop:");
            Regex ForLine = new Regex("^for:");
            Regex iInstructionLine = new Regex("^(bne|beq|sw|lw|addi)");//[bne|beq|sw|lw|addi]
            Regex rInstructionLine = new Regex("^(add|and|sub|nor|or|slt)");
            
            if (DataLine.IsMatch(TestOneLine))
            { //MessageBox.Show("DataLine");
                BeginData = DataLineWORK(TestOneLine, Datamemory, BeginData, DatamemoryNames);
            }
            else if (CommentLine.IsMatch(TestOneLine))
            {
               // MessageBox.Show("CommentLine");
            }
            else if (jInstructionLine.IsMatch(TestOneLine))
            {
                MessageBox.Show("--j->" + TestOneLine + "size" + BeginText.ToString());
                char[] jLine = new char[] { ' ' };//To split lines
                string[] jTwoArguments = TestOneLine.Split(jLine, StringSplitOptions.RemoveEmptyEntries);
                jmemorynames[jmemorycounter] = BeginText;
                jmemory[jmemorycounter] =jTwoArguments[1];
                jmemorycounter++;
                 BeginText++;
              //  MessageBox.Show("jInstructionLine");  
            }
           // else if (LoopLine.IsMatch(TestOneLine))
            
           // else if (ForLine.IsMatch(TestOneLine))
            
            else if (iInstructionLine.IsMatch(TestOneLine))
            {
                MessageBox.Show("--i->" + TestOneLine + "size" + BeginText.ToString());
                Textmemory[BeginText] = theIinstruction(TestOneLine, Datamemory, BeginData, DatamemoryNames, Textmemory, TheInstructions);
                BeginText++;
               // MessageBox.Show("iInstructionLine");
            }
            else if (rInstructionLine.IsMatch(TestOneLine))
            {
               // MessageBox.Show("rInstructionLine");
                Textmemory[BeginText] = theRinstruction(TestOneLine, Datamemory, BeginData, DatamemoryNames, Textmemory, TheInstructions);
                MessageBox.Show("--r->"+TestOneLine+"size"+BeginText.ToString());
                BeginText++;
            }
            else if ((TestOneLine.Length > 2)&&(TestOneLine[0]!='.'))
            {
                MessageBox.Show("--label->" + TestOneLine + "size" + BeginText.ToString());
                //MessageBox.Show("number line is  : " + di.ToString());
               // MessageBox.Show("length is : " + TestOneLine.Length.ToString());
               // MessageBox.Show("the line is  : [" + TestOneLine+"]");
                char[] TwoPoints = new char[] { ':' };//To split lines
                string[] TwoPointsUp = TestOneLine.Split(TwoPoints, StringSplitOptions.RemoveEmptyEntries);
                LabelsNames[LabelCounter] = TwoPointsUp[0];
                LabelsValues[LabelCounter] = BeginText;
                parts[di] = TwoPointsUp[1];
                di--;
                LabelCounter++;
            }
            
        }//while 
        for (int i = 0; i < jmemorycounter; i++)
        {
            Textmemory[jmemorynames[i]] = theJinstruction(jmemory[i], LabelsNames, LabelsValues, LabelCounter).Substring(0, 32);
             
        }
        for (int i = 0; i < BeginText; i++)
            {
             // MessageBox.Show("Textmemory  " + i.ToString() +"  "+Textmemory[i]);
            }
        MessageBox.Show("BeginText  " + BeginText.ToString() + "  ");
            WriteToFileText(Textmemory,   BeginText, false);
           
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
