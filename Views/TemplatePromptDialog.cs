using System.Drawing;
using System.Windows.Forms;
using DBClip.Models;

namespace DBClip.Views;

public class TemplatePromptDialog : Form
{
    private readonly List<string> _variables;
    private readonly RowContext _rowContext;
    private readonly List<TextBox> _textBoxes = new();
    private readonly List<Label> _labels = new();
    private Button btnOK;
    private Button btnCancel;
    private Button btnUseContext;
    private TableLayoutPanel tableLayoutPanel;

    public Dictionary<string, string> TemplateValues { get; } = new();

    public TemplatePromptDialog(List<string> variables, RowContext rowContext)
    {
        _variables = variables;
        _rowContext = rowContext;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.btnOK = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.btnUseContext = new System.Windows.Forms.Button();
        this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
        this.SuspendLayout();
        // 
        // tableLayoutPanel
        // 
        this.tableLayoutPanel.ColumnCount = 2;
        this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
        this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
        this.tableLayoutPanel.Location = new System.Drawing.Point(20, 20);
        this.tableLayoutPanel.Name = "tableLayoutPanel";
        this.tableLayoutPanel.RowCount = _variables.Count + 1;
        
        for (int i = 0; i < _variables.Count; i++)
        {
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
        }

        this.tableLayoutPanel.Size = new System.Drawing.Size(400, Math.Max(35 * _variables.Count, 100));
        this.tableLayoutPanel.TabIndex = 0;

        for (int i = 0; i < _variables.Count; i++)
        {
            var label = new Label
            {
                Text = _variables[i] + ":",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight
            };
            _labels.Add(label);
            tableLayoutPanel.Controls.Add(label, 0, i);

            var textBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Tag = _variables[i]
            };

            if (_rowContext.Values.TryGetValue(_variables[i], out var contextValue) && contextValue != null)
            {
                textBox.Text = contextValue.ToString();
            }

            _textBoxes.Add(textBox);
            tableLayoutPanel.Controls.Add(textBox, 1, i);
        }

        // 
        // btnUseContext
        // 
        this.btnUseContext.Location = new System.Drawing.Point(20, tableLayoutPanel.Bottom + 15);
        this.btnUseContext.Name = "btnUseContext";
        this.btnUseContext.Size = new System.Drawing.Size(130, 32);
        this.btnUseContext.TabIndex = 1;
        this.btnUseContext.Text = "Use Row Context";
        this.btnUseContext.UseVisualStyleBackColor = true;
        this.btnUseContext.Click += new System.EventHandler(this.btnUseContext_Click);
        // 
        // btnOK
        // 
        this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnOK.Location = new System.Drawing.Point(220, tableLayoutPanel.Bottom + 15);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new System.Drawing.Size(90, 32);
        this.btnOK.TabIndex = 2;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = true;
        this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(316, tableLayoutPanel.Bottom + 15);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(90, 32);
        this.btnCancel.TabIndex = 3;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // TemplatePromptDialog
        // 
        this.AcceptButton = this.btnOK;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(444, Math.Max(130 + 35 * _variables.Count, 160));
        this.Controls.Add(this.tableLayoutPanel);
        this.Controls.Add(this.btnUseContext);
        this.Controls.Add(this.btnOK);
        this.Controls.Add(this.btnCancel);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "TemplatePromptDialog";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Enter Template Values";
        this.ResumeLayout(false);
    }

    private void btnUseContext_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < _variables.Count; i++)
        {
            if (_rowContext.Values.TryGetValue(_variables[i], out var value) && value != null)
            {
                _textBoxes[i].Text = value.ToString() ?? "";
            }
        }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < _variables.Count; i++)
        {
            TemplateValues[_variables[i]] = _textBoxes[i].Text;
        }
    }
}
