import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
public class InstructionsGUI extends JFrame implements ActionListener{
    private JPanel mainPanel;
    private JPanel instructionsPanel;
    private JScrollPane instructionsScroll;
    private JLabel instructions;
    private JButton back;

    private String escapeFile = null;

    private Color backgroundColor = new Color(0xF2F2F2);
    private Color textColor = new Color(0x222222);
    private Color itemColor = new Color(0xC1C4C8);
    private Color terminalColor = new Color(0xDEEAFF);
    private Color selectedColor = new Color(0xC1CEE0);

    private Font font;
    private String fontName = "Sans-Serif";
    private String colorName = "Light";

    private int ftSize = 12;
    

    public InstructionsGUI(){
        

        setVisible(true);
        setSize(500,500);
        setLocationRelativeTo(null);
    }

    public InstructionsGUI(String filename, String name, Color b, Color txt, Color item, Color out, Color sel, String n, int sz){
        this.escapeFile = filename;

        this.colorName = name;
        this.backgroundColor = b;
        this.textColor = txt;
        this.itemColor = item;
        this.terminalColor = out;
        this.selectedColor = sel;
        this.fontName = n;
        this.ftSize = sz;

        mainPanel = new JPanel();
        instructionsPanel = new JPanel();
        instructions = new JLabel("<html><h1 id=\"how-to-create-a-custom-escape-room\">How to Create a Custom Escape Room</h1><p>In order to create an escape room, users must create a .txt file in a specific format. <br>It is very important that users use the exact formatting required when creating an escape room because the program does not have much handling for incorrect formatting. <br>It is the responsibility of the user to make sure they create the escape room file properly. Please refer to the end of this file for an example. </p><h2 id=\"escape-room-elements-descriptions\">Escape Room Elements Descriptions</h2><ul><li>An escape room is made up of several elements. These are:<ul><li>Beginning Script. These lines will start with &quot;Beginning: &quot; which will set the script that is displayed at the beginning of the escape room. </li><li>Ending Script. These lines will start with &quot;End: &quot; which will set the script that is displayed when the player escapes from the escape room. </li><li>Room. These lines will start with &quot;Room: &quot; which will tell the program to parse the line as a room. A room is made up of several parts. They must be entered in a specific order which I will list now. <br>Each part must be separated from the rest of the line with a pipe character &quot;|&quot;. Please note that none of these may contain a pipe character as part of the entry.<ul><li>Name. Enter the name of the room. </li><li>Script. This is a description of the room that you want to pop up when the player enters the room. </li><li>Requires Key. If this room requires a key to enter, type &quot;true&quot;, if not type &quot;false&quot;.</li><li>End. If this room is the last room in the escape room, type &quot;true&quot;, if not type &quot;false&quot;.</li><li>Image Path. Type the image path of the image that you would like displayed when the player is in this room. </li><li>Code. Type the key code for the room. If there is no code for this room, type null. Please note that you cannot have a key for a room be &quot;null&quot; as that is not compatible with this program. </li><li>Connected Rooms. Please enter the names of the rooms that can be accessed from within this room. Separate each name with a comma so that the program can distinguish between them. </li><li>Keys. Please enter the names of the keys that are within the room, separate each name with a space so that the program can distinguish between them. </li></ul></li><li>Key. These lines will start with &quot;Key: &quot;. Then you will enter the name of the key, followed a pipe character &quot;|&quot; and a list of the names of rooms that that key unlocks, separated by commas.</li><li>Map. These lines will start with &quot;Map: &quot; and then will have the filepath to a file that contains the image you would like displayed as the overall map for your escape room. </li></ul></li></ul><h2 id=\"example-escape-room\">Example Escape Room</h2><pre><code>Beginning: This is a description of the beginning of the escape room. It will describe <span class=\"hljs-keyword\">where</span> I am and how I came to be unfortunately trapped there.<span class=\"hljs-keyword\"><br>End</span>: This is a description of the <span class=\"hljs-keyword\">end</span>. Whew! Glad I got out of that place...<br>Room: Bathroom|<span class=\"hljs-type\">This</span> is a description of the bathroom|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">/image</span>.png|<span class=\"hljs-type\">ABC</span>|<span class=\"hljs-type\">Library</span>,Bedroom,Kitchen|<span class=\"hljs-type\">Key1</span>,Key2|<span class=\"hljs-type\"><br>Room</span>: <span class=\"hljs-keyword\">Library</span>|<span class=\"hljs-type\">This</span> is <span class=\"hljs-keyword\">where</span> they keep all of the books|<span class=\"hljs-type\">true</span>|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">/image</span>.png|<span class=\"hljs-type\">ABC</span>|<span class=\"hljs-type\">Library</span>,Bedroom,Kitchen|<span class=\"hljs-type\">Key1</span>,Key2|<span class=\"hljs-type\"><br>Room</span>: Kitchen|<span class=\"hljs-type\">This</span> is <span class=\"hljs-keyword\">where</span> they <span class=\"hljs-built_in\">do</span> all of the cooking|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">/image</span>.png|<span class=\"hljs-type\">ABC</span>|<span class=\"hljs-type\">Library</span>,Bedroom,Kitchen|<span class=\"hljs-type\">Key1</span>,Key2|<span class=\"hljs-type\"><br>Room</span>: Bedroom|<span class=\"hljs-type\">This</span> is <span class=\"hljs-keyword\">where</span> the person sleeps|<span class=\"hljs-type\">true</span>|<span class=\"hljs-type\">true</span>|<span class=\"hljs-type\">/image</span>.png|<span class=\"hljs-type\">ABC</span>|<span class=\"hljs-type\">Library</span>,Bedroom,Kitchen|<span class=\"hljs-type\">Key1</span>,Key2|<span class=\"hljs-type\"><br>Key</span>: Key1|<span class=\"hljs-type\">Bathroom</span>,Bedroom<br>Key: Key2|<span class=\"hljs-type\">Kitchen</span><br>Map: /image.png</code></pre><h2 id=\"some-important-considerations\">Some Important Considerations</h2><ul><li>Please note that each line must start with one of the listed keys for the elements and nothing may contain any &quot;\\n&quot; newline characters.</li><li>You must follow these instructions exactly in order to create an effective escape room.</li><li>There must not be any blank lines in your file. </li><li>Make sure you have the spacing/punctuation in your file exactly the same as you would like it to show up in your escape room. </li><li>Please note that all line starting keywords must have a colon and a space before you write any other elements. </li></ul></html>");
        back = new JButton("Back");

        instructionsScroll = new JScrollPane(instructions);

        back.addActionListener(this);

        instructionsScroll.setPreferredSize(new Dimension(1400, 650));
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        instructionsPanel.add(instructionsScroll);
        
        mainPanel.add(instructionsPanel);
        mainPanel.add(back);

        add(mainPanel);

        setVisible(true);
        setSize(1500,750);
        setLocationRelativeTo(null);
    }

    public void actionPerformed(ActionEvent e){
        Object comp = e.getSource();
        if(back == comp){
            this.dispose();
        }
    }
}
