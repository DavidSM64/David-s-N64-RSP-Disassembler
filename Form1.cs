using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace N64_RSP_DISASSEMBLER
{
    public partial class Form1 : Form
    {
        byte[] rom = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_ROM_ADDRESS.Tag = textBox_ROM_ADDRESS.Text;
            textBox_RAM_ADDRESS.Tag = textBox_RAM_ADDRESS.Text;
            textBox_DATA_LENGTH.Tag = textBox_DATA_LENGTH.Text;
            STRING_ROM_ADDRESS = textBox_ROM_ADDRESS.Text;
            STRING_RAM_ADDRESS = textBox_RAM_ADDRESS.Text;
            STRING_DATA_LENGTH = textBox_DATA_LENGTH.Text;
        }

        private void button_loadROM_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "BIN|*.bin|Z64 ROM|*.z64|V64 ROM|*.v64|N64 ROM|*.n64";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                rom = File.ReadAllBytes(openFileDialog1.FileName);
            }
            else
            {
                return;
            }
        }

        public byte[] getSubArray(byte[] arr, uint offset, uint size)
        {
            if (offset + size >= arr.Length)
            {
                size = (uint)(arr.Length - offset);
            }
            byte[] newArr = new byte[size];
            Array.Copy(arr, offset, newArr, 0, size);
            return newArr;
        }

        public uint getWordFromArray(byte[] bytes, uint offset)
        {
            return (uint)(bytes[0 + offset] << 24 | bytes[1 + offset] << 16
                | bytes[2 + offset] << 8 | bytes[3 + offset]);
        }

        private string STRING_ROM_ADDRESS = "";
        private string STRING_RAM_ADDRESS = "";
        private string STRING_DATA_LENGTH = "";

        private void decodeRSPSection()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Courier New;}}{\\colortbl;\\red0\\green0\\blue0;\\red32\\green100\\blue32;\\red32\\green32\\blue100; }\\viewkind4\\uc1\\pard\\f0\\fs17 ");


            byte[] data = getSubArray(rom, Convert.ToUInt32(STRING_ROM_ADDRESS, 16), 
                Convert.ToUInt32(STRING_DATA_LENGTH, 16));

            int num_instr = data.Length / 4;

            for (uint i = 0; i < num_instr; i++)
            {
                uint cur_ram_address = Convert.ToUInt32(STRING_RAM_ADDRESS, 16) + (i * 4);
                uint cur_rom_address = Convert.ToUInt32(STRING_ROM_ADDRESS, 16) + (i * 4);
                uint operation = getWordFromArray(data, i * 4);
                if (cb_showAddresses.Checked)
                {
                    sb.Append("\\cf2 "); // Set to color #2 (Green)
                    sb.Append("[");
                    sb.Append(cur_ram_address.ToString("X8"));
                    sb.Append(" / ");
                    sb.Append(cur_rom_address.ToString("X"));
                    sb.Append("] ");
                }
                if (cb_showBinary.Checked)
                {
                    sb.Append("\\cf3 "); // Set to color #3 (Blue)
                    sb.Append(operation.ToString("X8"));
                    sb.Append("\\cf1 "); // Set to color #1 (Black)
                    sb.Append(" | ");
                }
                else
                {
                    sb.Append("\\cf1 "); // Set to color #1 (Black)
                }
                sb.Append(RSP_DECODER.decodeOPERATION(operation, cur_ram_address, cb_regNames.Checked));
                sb.Append("\\line "); // Add a new line
            }
            sb.Append("}");
            textBox.Rtf = sb.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rom != null)
            {
                STRING_ROM_ADDRESS = textBox_ROM_ADDRESS.Text;
                STRING_RAM_ADDRESS = textBox_RAM_ADDRESS.Text;
                STRING_DATA_LENGTH = textBox_DATA_LENGTH.Text;
                decodeRSPSection();
            }
        }

        private void cb_showBinary_CheckedChanged(object sender, EventArgs e)
        {
            if (rom != null)
            {
                decodeRSPSection();
            }
        }
        
        private void textBox_DATA_LENGTH_TextChanged(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            string previousText = "";
            int selectedStart = box.SelectionStart;
            if (box.Tag != null)
            {
                previousText = (string)box.Tag;
            }
            
            // Make sure that the textbox only has hex characters
            if (System.Text.RegularExpressions.Regex.IsMatch(box.Text, "[^0-9A-Fa-f]"))
            {
                box.Text = previousText;
                box.SelectionStart = selectedStart - 1;
            }

            box.Tag = box.Text;
        }
    }
}
