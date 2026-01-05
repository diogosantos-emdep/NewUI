using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
namespace Emdep.Geos.Modules.HarnessPart.Class
{
  public  class WTGLoad
    {
        /// <summary>
        /// Read WTG image file by pass to this method <see cref="System.IO.Stream"/> variable. 
        /// </summary>
        /// <param name="s"> WTG image file stream</param>
        /// <returns>
        /// Returns <paramref name="imageLoad"/> WTG <see cref="System.Drawing.Image"/> to load in viewer.
        /// </returns>
        /// <value>
        /// Required valid .wtg file stream.
        /// </value>
        /// <remarks>
        /// This method is use to read data of WTG file by passing stream.
        /// </remarks>
        /// <example>
        /// <code>
        /// 
        /// Stream s = new FileStream(WTGfilePath, FileMode.Open, FileAccess.Read);
        /// Image img = WTGLoad.ReadWTG(s);     //using PaintingToolsBL;
        /// 
        /// </code>
        /// </example>
        public static Image ReadWTG(Stream s)
        {
            Image imageLoad = null;
            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            //Stream s = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            //byte[] Fix_Byte = Modules.Test.ReadFully(s, 0);
            //byte[] File_Size = Modules.Test.ReadFully(s, 4);
            //byte[] pic_property0 = Modules.Test.ReadFully(s, 8);
            //byte[] pic_property1 = Modules.Test.ReadFully(s, 8);
            //byte[] pic_property2 = Modules.Test.ReadFully(s, 8);
            //byte[] pic_property3 = Modules.Test.ReadFully(s, 8);
            //byte[] pic_property4 = Modules.Test.ReadFully(s, 8);



            // Reads data from a stream start at 20th byte which have "pic" property name
            s.Seek(20, SeekOrigin.Begin);
            byte[] pic_property = ReadFully(s, 4);
            string pic_property_name = BitConverter.ToString(pic_property, 0);
            //Stream s_pic_name = new MemoryStream(pic_property);
            //string pic_property_name = s_pic_name.ToString();

            //byte[] BM_Head0 = Modules.Test.ReadFully(s, 8);
            //byte[] BM_Head1 = Modules.Test.ReadFully(s, 8);
            //byte[] BM_Head2 = Modules.Test.ReadFully(s, 8);
            //byte[] BM_Head3 = Modules.Test.ReadFully(s, 8);
            //byte[] BM_Head4 = Modules.Test.ReadFully(s, 8);
            //byte[] BM_Head5 = Modules.Test.ReadFully(s, 8);
            //byte[] BM_Head6 = Modules.Test.ReadFully(s, 8);

            // Reads data from a stream to find Length of BMP file at 52 to 55 byte which have position after 2 byte of "BM" tag

            //s.Seek(2 , SeekOrigin.Begin);     //Start byte of BMP image size
            s.Seek(52, SeekOrigin.Begin);       //Start byte of BMP image size in WTG file
            byte[] BM_Size = new byte[sizeof(int)];
            BM_Size = ReadFully(s, sizeof(int));
            int bm_size = BitConverter.ToInt32(BM_Size, 0);

            // Reads data from a stream start point of BMP file at 50th byte which have "BM" Start of 

            //s.Seek(0, SeekOrigin.Begin);      // Start byte for BMP file
            s.Seek(50, SeekOrigin.Begin);       // Start byte for BMP content in WTG file
            byte[] BM_Head = ReadFully(s, bm_size + 50);

            // Reads data from a byte and convert to Stream
            Stream s_Image = new MemoryStream(BM_Head);

            // Reads data from a stream to find count of object near tag "nobjetos" in WTG file.
            s.Seek(bm_size + 50 + 12, SeekOrigin.Begin);                //Start byte of tag "nobjetos" in WTG file
            byte[] WTG_nObject = new byte[16];
            WTG_nObject = ReadFully(s, 16);

            byte[] WTG_nObject_count = new byte[4];
            WTG_nObject_count = ReadFully(s, 4);
           // int nObject_count = BitConverter.ToInt32(WTG_nObject_count, 0);
            int nObject_count = 0;
            WTGValues.NObject = nObject_count.ToString();      // Get and Set object's count in WTG file

            int objcount = 1;//nObject_count;

            byte[] WTG_nObject_count1 = new byte[4];
            WTG_nObject_count1 = ReadFully(s, 4);

            byte[] WTG_nObject_count2 = new byte[4];
            WTG_nObject_count2 = ReadFully(s, 4);

            byte[] WTG_nObject_count3 = new byte[4];
            WTG_nObject_count3 = ReadFully(s, 4);

            // Reads data from a stream to find count of identlabel near tag "identlabel" in WTG file.
            byte[] WTG_identlable = new byte[20];
            WTG_identlable = ReadFully(s, 20);
            string identlable = enc.GetString(WTG_identlable);

            byte[] WTG_identlable_count = new byte[4];
            WTG_identlable_count = ReadFully(s, 4);
            int identlable_count = 0;
               //= BitConverter.ToInt32(WTG_identlable_count, 0);
          // identlable_count = 0;
            WTGValues.IdentLabel = identlable_count.ToString();      // Get and Set identlabel's count in WTG file

            byte[] WTG_identlable3 = new byte[4];
            WTG_identlable3 = ReadFully(s, 4);

            byte[] WTG_identlable4 = new byte[4];
            WTG_identlable4 = ReadFully(s, 4);

            byte[] WTG_identlable5 = new byte[4];
            WTG_identlable5 = ReadFully(s, 4);

            // ArrayList for all n object
            WTGValues.ObjectName = new ArrayList();
            WTGValues.ObjectDestination = new Dictionary<string, PointF>();
            WTGValues.ObjectOrigin = new Dictionary<string, PointF>();
            WTGValues.ObjectRadius = new Dictionary<string, short>();
            WTGValues.ObjectType = new Dictionary<string, byte>();
            WTGValues.ObjectVisible = new Dictionary<string, bool>();
            WTGValues.ObjectTypePiece = new Dictionary<string, byte>();
            WTGValues.ObjectColor = new Dictionary<string, int[]>();

            ArrayList al_obj_dstn_x = new ArrayList();
            ArrayList al_obj_dstn_y = new ArrayList();
            ArrayList al_obj_labelcaption = new ArrayList();
            ArrayList al_obj_orgn_x = new ArrayList();
            ArrayList al_obj_orgn_y = new ArrayList();
            ArrayList al_obj_rdo = new ArrayList();
            ArrayList al_obj_tipoobjeto = new ArrayList();
            ArrayList al_obj_visible = new ArrayList();
            ArrayList al_obj_tipopieza = new ArrayList();
            ArrayList al_obj_r = new ArrayList();
            ArrayList al_obj_g = new ArrayList();
            ArrayList al_obj_bl = new ArrayList();


            //byte[] bt = new byte[s.Length];

            //MemoryStream ms = new MemoryStream();
            //int ct = (int) s.Length;
            //int read = s.Read(bt, 0, ct);
            //ms.Write(bt, 0, read);

            //Stream bstream = new MemoryStream(bt);
            //BinaryFormatter bformatter = new BinaryFormatter();
            //ObjectoValues statev = new ObjectoValues();
            //statev.objectn = (ObjectoValues)bformatter.Deserialize(bstream);

            Dictionary<string, PointF> d = new Dictionary<string, PointF>();

            while (objcount <= Convert.ToInt32(WTGValues.NObject))
            {
                string stop;
                if (objcount == 2)
                {
                    stop = objcount.ToString();
                }

                short objeto_nLenght = 0;

                if (objcount > 9)
                {
                    objeto_nLenght = 2;
                }
                if (objcount > 99)
                {
                    objeto_nLenght = 4;
                }


                // Reads data from a stream to find single shape "WAY" data in 1252 byte

                //byte[] WTG_objeto = new byte[1252];
                //WTG_objeto = ReadFully(s, 1252);
                //int objeto = BitConverter.ToInt32(WTG_objeto, 0);
                byte[] WTG_objeto = new byte[14 + objeto_nLenght];
                WTG_objeto = ReadFully(s, 14 + objeto_nLenght);
                string objeto = enc.GetString(WTG_objeto);      //o\0b\0j\0e\0t\0o\09\0
                WTGValues.ObjectName.Add(objeto.Replace("\0", ""));


                byte[] WTG_objeto1 = new byte[8];
                WTG_objeto1 = ReadFully(s, 8);

                byte[] WTG_objeto2 = new byte[8];
                WTG_objeto2 = ReadFully(s, 8);

                byte[] WTG_objeto3 = new byte[14];
                WTG_objeto3 = ReadFully(s, 14);

                // Reads data from a stream to find start of  "destino" data in 16 byte
                byte[] WTG_objeto_destino = new byte[16];
                WTG_objeto_destino = ReadFully(s, 16);
                string destino = enc.GetString(WTG_objeto_destino);     //d\0e\0s\0t\0i\0n\0o\0\0
                //int objeto_destino = BitConverter.ToInt32(WTG_objeto_destino, 0);

                byte[] WTG_objeto5 = new byte[20];
                WTG_objeto5 = ReadFully(s, 20);

                byte[] WTG_objeto6 = new byte[8];
                WTG_objeto6 = ReadFully(s, 8);

                // Reads data from a stream to find start of  "x" of "destino"
                byte[] WTG_objeto_destino_x = new byte[2];
                WTG_objeto_destino_x = ReadFully(s, 2);
                string destino_x = enc.GetString(WTG_objeto_destino_x);

                byte[] WTG_objeto_destino_x_value = new byte[4];
                WTG_objeto_destino_x_value = ReadFully(s, 4);
                Single WTG_objeto_destino_x_position = BitConverter.ToSingle(WTG_objeto_destino_x_value, 0);

                PointF destination = new PointF();

                al_obj_dstn_x.Add(WTG_objeto_destino_x_position);

                destination.X = WTG_objeto_destino_x_position;

                byte[] WTG_objeto_destino_x1 = new byte[6];
                WTG_objeto_destino_x1 = ReadFully(s, 6);

                // Reads data from a stream to find start of  "y" of "destino"
                byte[] WTG_objeto_destino_y = new byte[2];
                WTG_objeto_destino_y = ReadFully(s, 2);
                string destino_y = enc.GetString(WTG_objeto_destino_y);

                byte[] WTG_objeto_destino_y1 = new byte[8];
                WTG_objeto_destino_y1 = ReadFully(s, 8);

                byte[] WTG_objeto_destino_y_value = new byte[4];
                WTG_objeto_destino_y_value = ReadFully(s, 4);
                Single WTG_objeto_destino_y_position = BitConverter.ToSingle(WTG_objeto_destino_y_value, 0);

                al_obj_dstn_y.Add(WTG_objeto_destino_y_position);

                destination.Y = WTG_objeto_destino_y_position;

                d.Add(objeto.Replace("\0", ""), destination);

                WTGValues.ObjectDestination.Add(objeto.Replace("\0", ""), destination);


                byte[] WTG_objeto_destino_y2 = new byte[14];
                WTG_objeto_destino_y2 = ReadFully(s, 14);

                // Reads data from a stream to find start of  "estaseleccionado"
                byte[] WTG_objeto_estaseleccionado = new byte[32];
                WTG_objeto_estaseleccionado = ReadFully(s, 32);
                string estaseleccionado = enc.GetString(WTG_objeto_estaseleccionado);

                byte[] WTG_objeto_estaseleccionado1 = new byte[16];
                WTG_objeto_estaseleccionado1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "extraidentificado"
                byte[] WTG_objeto_extraidentificado = new byte[36];
                WTG_objeto_extraidentificado = ReadFully(s, 36);
                string extraidentificado = enc.GetString(WTG_objeto_extraidentificado);

                byte[] WTG_objeto_extraidentificado1 = new byte[12];
                WTG_objeto_extraidentificado1 = ReadFully(s, 12);

                // Reads data from a stream to find start of  "labelcaption "
                byte[] WTG_objeto_labelcaption = new byte[24];
                WTG_objeto_labelcaption = ReadFully(s, 24);
                string labelcaption = enc.GetString(WTG_objeto_labelcaption);       //l\0a\0b\0e\0l\0c\0a\0p\0t\0i\0o\0n\0

                byte[] WTG_objeto_labelcaption1 = new byte[4];      // lenght of lablecaption
                WTG_objeto_labelcaption1 = ReadFully(s, 4);
                int lablecaption_lenght = BitConverter.ToInt32(WTG_objeto_labelcaption1, 0);

                byte[] WTG_objeto_labelcaption_value = new byte[(lablecaption_lenght * 2)];
                WTG_objeto_labelcaption_value = ReadFully(s, (lablecaption_lenght * 2));
                string WTG_objeto_labelcaption_value_string = BitConverter.ToString(WTG_objeto_labelcaption_value, 0);
                string obj_labcap = enc.GetString(WTG_objeto_labelcaption_value);

                al_obj_labelcaption.Add(obj_labcap);

                short lablecaption_Lt = 0;
                if (lablecaption_lenght % 2 == 0)
                {
                    //Even Number
                    lablecaption_Lt = 12;
                }
                else
                {
                    //Odd Number
                    lablecaption_Lt = 14;
                }

                byte[] WTG_objeto_labelcaption2 = new byte[lablecaption_Lt];
                WTG_objeto_labelcaption2 = ReadFully(s, lablecaption_Lt);

                // Reads data from a stream to find start of  "labelname "
                byte[] WTG_objeto_labelname = new byte[18];
                WTG_objeto_labelname = ReadFully(s, 18);
                string labelname = enc.GetString(WTG_objeto_labelname);

                byte[] WTG_objeto_labelname1 = new byte[4];
                WTG_objeto_labelname1 = ReadFully(s, 4);
                int labelname_lenght = BitConverter.ToInt32(WTG_objeto_labelname1, 0);

                // Reads data from a stream to find start of  "m_label<n>"
                byte[] WTG_objeto_m_label1 = new byte[labelname_lenght * 2];
                WTG_objeto_m_label1 = ReadFully(s, labelname_lenght * 2);
                string m_label1 = enc.GetString(WTG_objeto_m_label1);

                short m_label_Lt = 0;
                if (labelname_lenght % 2 == 0)
                {
                    //Even Number
                    m_label_Lt = 14;
                }
                else
                {
                    //Odd Number
                    m_label_Lt = 12;
                }

                byte[] WTG_objeto_m_label1_1 = new byte[m_label_Lt];
                WTG_objeto_m_label1_1 = ReadFully(s, m_label_Lt);

                // Reads data from a stream to find start of  "labelheight"
                byte[] WTG_objeto_m_label1_labelheight = new byte[22];
                WTG_objeto_m_label1_labelheight = ReadFully(s, 22);
                string labelheight = enc.GetString(WTG_objeto_m_label1_labelheight);

                byte[] WTG_objeto_m_label1_labelheight2 = new byte[2];
                WTG_objeto_m_label1_labelheight2 = ReadFully(s, 2);
                int lable_height = BitConverter.ToInt16(WTG_objeto_m_label1_labelheight2, 0);

                if (lable_height == 200)
                {
                    byte[] WTG_objeto_m_label1_labelheight1 = new byte[12];
                    WTG_objeto_m_label1_labelheight1 = ReadFully(s, 12);
                }
                else
                {
                    byte[] WTG_objeto_m_label1_labelheight1 = new byte[16];
                    WTG_objeto_m_label1_labelheight1 = ReadFully(s, 16);
                }

                // Reads data from a stream to find start of  "labelwidth"
                byte[] WTG_objeto_m_label1_labelwidth = new byte[20];
                WTG_objeto_m_label1_labelwidth = ReadFully(s, 20);
                string labelwidth = enc.GetString(WTG_objeto_m_label1_labelwidth);


                byte[] WTG_objeto_m_label1_labelwidth1 = new byte[2];
                WTG_objeto_m_label1_labelwidth1 = ReadFully(s, 2);
                int lable_width = BitConverter.ToInt16(WTG_objeto_m_label1_labelheight2, 0);

                if (lable_width == 200)
                {
                    byte[] WTG_objeto_m_label1_labelwidth2 = new byte[14];
                    WTG_objeto_m_label1_labelwidth2 = ReadFully(s, 14);
                }
                else
                {
                    byte[] WTG_objeto_m_label1_labelwidth2 = new byte[14];
                    WTG_objeto_m_label1_labelwidth2 = ReadFully(s, 14);
                }

                // Reads data from a stream to find start of  "isblinking"
                byte[] WTG_objeto_isblinking = new byte[20];
                WTG_objeto_isblinking = ReadFully(s, 20);
                string isblinking = enc.GetString(WTG_objeto_isblinking);

                byte[] WTG_objeto_isblinking1 = new byte[16];
                WTG_objeto_isblinking1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "moviendose"
                byte[] WTG_objeto_moviendose = new byte[20];
                WTG_objeto_moviendose = ReadFully(s, 20);
                string moviendose = enc.GetString(WTG_objeto_moviendose);

                byte[] WTG_objeto_moviendose1 = new byte[16];
                WTG_objeto_moviendose1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "origen"
                byte[] WTG_objeto_origen = new byte[12];
                WTG_objeto_origen = ReadFully(s, 12);
                string origen = enc.GetString(WTG_objeto_origen);

                byte[] WTG_objeto_origen1 = new byte[22];
                WTG_objeto_origen1 = ReadFully(s, 22);

                // Reads data from a stream to find start of 1st "x" of "origen" 
                byte[] WTG_objeto_origen1_x0 = new byte[8];
                WTG_objeto_origen1_x0 = ReadFully(s, 8);
                string origen_x0 = enc.GetString(WTG_objeto_origen1_x0);

                // Reads data from a stream to find start of 2nd "x" of "origen" 
                byte[] WTG_objeto_origen1_x = new byte[2];
                WTG_objeto_origen1_x = ReadFully(s, 2);
                string origen_x = enc.GetString(WTG_objeto_origen1_x);

                byte[] WTG_objeto_origen1_x1 = new byte[4];
                WTG_objeto_origen1_x1 = ReadFully(s, 4);
                Single WTG_objeto_origen_x_value = BitConverter.ToSingle(WTG_objeto_origen1_x1, 0);

                PointF origin = new PointF();

                al_obj_orgn_x.Add(WTG_objeto_origen_x_value);

                origin.X = WTG_objeto_origen_x_value;

                byte[] WTG_objeto_origen1_x2 = new byte[6];
                WTG_objeto_origen1_x2 = ReadFully(s, 6);

                // Reads data from a stream to find start of 2nd "y" of "origen" 
                byte[] WTG_objeto_origen1_y = new byte[2];
                WTG_objeto_origen1_y = ReadFully(s, 2);
                string origen_y = enc.GetString(WTG_objeto_origen1_y);

                byte[] WTG_objeto_origen1_y1 = new byte[8];
                WTG_objeto_origen1_y1 = ReadFully(s, 8);

                byte[] WTG_objeto_origen1_y_value = new byte[4];
                WTG_objeto_origen1_y_value = ReadFully(s, 4);
                Single WTG_objeto_origen_y_value = BitConverter.ToSingle(WTG_objeto_origen1_y_value, 0);

                al_obj_orgn_y.Add(WTG_objeto_origen_y_value);

                origin.Y = WTG_objeto_origen_y_value;

                WTGValues.ObjectOrigin.Add(objeto.Replace("\0", ""), origin);

                byte[] WTG_objeto_origen2 = new byte[16];
                WTG_objeto_origen2 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "radio"
                byte[] WTG_objeto_radio = new byte[10];
                WTG_objeto_radio = ReadFully(s, 10);
                string radio = enc.GetString(WTG_objeto_radio);

                byte[] WTG_objeto_radio1 = new byte[2];
                WTG_objeto_radio1 = ReadFully(s, 2);
                int WTG_objeto_radio1_value = BitConverter.ToInt16(WTG_objeto_radio1, 0);

                al_obj_rdo.Add(WTG_objeto_radio1_value);

                WTGValues.ObjectRadius.Add(objeto.Replace("\0", ""), Convert.ToInt16(WTG_objeto_radio1_value));

                byte[] WTG_objeto_radio2 = new byte[12];
                WTG_objeto_radio2 = ReadFully(s, 12);

                // Reads data from a stream to find start of  "redimensionando"
                byte[] WTG_objeto_redimensionando = new byte[30];
                WTG_objeto_redimensionando = ReadFully(s, 30);
                string redimensionando = enc.GetString(WTG_objeto_redimensionando);

                byte[] WTG_objeto_redimensionando1 = new byte[14];  //[12]
                WTG_objeto_redimensionando1 = ReadFully(s, 14);     //[12]

                // Reads data from a stream to find start of  "relleno"
                byte[] WTG_objeto_relleno = new byte[14];       //[14]
                WTG_objeto_relleno = ReadFully(s, 14);          //[14]
                string relleno = enc.GetString(WTG_objeto_relleno);

                byte[] WTG_objeto_relleno1 = new byte[14];
                WTG_objeto_relleno1 = ReadFully(s, 14);

                // Reads data from a stream to find start of  "rellenoAux"
                byte[] WTG_objeto_rellenoAux = new byte[20];
                WTG_objeto_rellenoAux = ReadFully(s, 20);
                string rellenoAux = enc.GetString(WTG_objeto_rellenoAux);

                byte[] WTG_objeto_rellenoAux1 = new byte[16];
                WTG_objeto_rellenoAux1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "tipoobjeto"
                byte[] WTG_objeto_tipoobjeto = new byte[20];
                WTG_objeto_tipoobjeto = ReadFully(s, 20);
                string tipoobjeto = enc.GetString(WTG_objeto_tipoobjeto);

                byte[] WTG_objeto_tipoobjeto2 = new byte[4];
                WTG_objeto_tipoobjeto2 = ReadFully(s, 4);
                int WTG_objeto_tipoobjeto1_value = BitConverter.ToInt32(WTG_objeto_tipoobjeto2, 0);

                al_obj_tipoobjeto.Add(WTG_objeto_tipoobjeto1_value);

                WTGValues.ObjectType.Add(objeto.Replace("\0", ""), Convert.ToByte(WTG_objeto_tipoobjeto1_value));


                byte[] WTG_objeto_tipoobjeto3 = new byte[12];
                WTG_objeto_tipoobjeto3 = ReadFully(s, 12);

                // Reads data from a stream to find start of  "visible"
                byte[] WTG_objeto_visible = new byte[14];
                WTG_objeto_visible = ReadFully(s, 14);
                string visible = enc.GetString(WTG_objeto_visible);

                byte[] WTG_objeto_visible1 = new byte[2];
                WTG_objeto_visible1 = ReadFully(s, 2);
                Boolean WTG_obj_visible = BitConverter.ToBoolean(WTG_objeto_visible1, 0);
                al_obj_visible.Add(WTG_obj_visible);

                WTGValues.ObjectVisible.Add(objeto.Replace("\0", ""), WTG_obj_visible);

                byte[] WTG_objeto_visible2 = new byte[12];
                WTG_objeto_visible2 = ReadFully(s, 12);

                // Reads data from a stream to find start of  "pintocables"
                byte[] WTG_objeto_pintocables = new byte[22];
                WTG_objeto_pintocables = ReadFully(s, 22);
                string pintocables = enc.GetString(WTG_objeto_pintocables);

                byte[] WTG_objeto_pintocables1 = new byte[14];
                WTG_objeto_pintocables1 = ReadFully(s, 14);

                // Reads data from a stream to find start of  "numorden"
                byte[] WTG_objeto_numorden = new byte[16];
                WTG_objeto_numorden = ReadFully(s, 16);
                string numorden = enc.GetString(WTG_objeto_numorden);

                byte[] WTG_objeto_numorden1 = new byte[16];
                WTG_objeto_numorden1 = ReadFully(s, 16);


                // Reads data from a stream to find start of  "tipopieza"
                byte[] WTG_objeto_tipopieza = new byte[18];
                WTG_objeto_tipopieza = ReadFully(s, 18);
                string tipopieza = enc.GetString(WTG_objeto_tipopieza);

                byte[] WTG_objeto_tipopieza1 = new byte[2];
                WTG_objeto_tipopieza1 = ReadFully(s, 2);
                int WTG_objeto_tipopieza1_value = BitConverter.ToInt16(WTG_objeto_tipopieza1, 0);

                WTGValues.ObjectTypePiece.Add(objeto.Replace("\0", ""), Convert.ToByte(WTG_objeto_tipopieza1_value));

                al_obj_tipopieza.Add(WTG_objeto_tipopieza1_value);

                byte[] WTG_objeto_tipopieza2 = new byte[6];
                WTG_objeto_tipopieza2 = ReadFully(s, 6);

                // Reads data from a stream to find start of  "r"
                byte[] WTG_objeto_r = new byte[4];
                WTG_objeto_r = ReadFully(s, 4);

                byte[] WTG_objeto_r1 = new byte[6];
                WTG_objeto_r1 = ReadFully(s, 6);

                byte[] WTG_objeto_r2 = new byte[2];
                WTG_objeto_r2 = ReadFully(s, 2);
                string r = enc.GetString(WTG_objeto_r2);

                byte[] WTG_objeto_r3 = new byte[2];
                WTG_objeto_r3 = ReadFully(s, 2);
                int WTG_objeto_r3_value = BitConverter.ToInt16(WTG_objeto_r3, 0);

                int[] RGB = new int[3];

                al_obj_r.Add(WTG_objeto_r3_value);

                RGB[0] = WTG_objeto_r3_value;

                byte[] WTG_objeto_r4 = new byte[2];
                WTG_objeto_r4 = ReadFully(s, 2);

                // Reads data from a stream to find start of  "g"
                byte[] WTG_objeto_g = new byte[10];
                WTG_objeto_g = ReadFully(s, 10);

                byte[] WTG_objeto_g1 = new byte[2];
                WTG_objeto_g1 = ReadFully(s, 2);
                string g = enc.GetString(WTG_objeto_g1);

                byte[] WTG_objeto_g3 = new byte[2];
                WTG_objeto_g3 = ReadFully(s, 2);
                int WTG_objeto_g3_value = BitConverter.ToInt16(WTG_objeto_g3, 0);

                al_obj_g.Add(WTG_objeto_g3_value);

                RGB[1] = WTG_objeto_g3_value;

                byte[] WTG_objeto_g2 = new byte[2];
                WTG_objeto_g2 = ReadFully(s, 2);

                // Reads data from a stream to find start of  "bl"
                byte[] WTG_objeto_bl = new byte[4];
                WTG_objeto_bl = ReadFully(s, 4);

                byte[] WTG_objeto_bl1 = new byte[6];
                WTG_objeto_bl1 = ReadFully(s, 6);

                byte[] WTG_objeto_bl2 = new byte[4];                //start of  "bl"
                WTG_objeto_bl2 = ReadFully(s, 4);
                string b = enc.GetString(WTG_objeto_bl2);

                byte[] WTG_objeto_bl3 = new byte[2];
                WTG_objeto_bl3 = ReadFully(s, 2);
                int WTG_objeto_bl3_value = BitConverter.ToInt16(WTG_objeto_bl3, 0);

                RGB[2] = WTG_objeto_bl3_value;

                al_obj_bl.Add(WTG_objeto_bl3_value);

                WTGValues.ObjectColor.Add(objeto.Replace("\0", ""), RGB);

                byte[] WTG_objeto_bl4 = new byte[14];
                WTG_objeto_bl4 = ReadFully(s, 14);


                // Reads data from a stream to find start of  "colorcable1r"
                byte[] WTG_objeto_colorcable1r = new byte[24];
                WTG_objeto_colorcable1r = ReadFully(s, 24);
                string colorcable1r = enc.GetString(WTG_objeto_colorcable1r);

                byte[] WTG_objeto_colorcable1r1 = new byte[16];
                WTG_objeto_colorcable1r1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "colorcable1g"
                byte[] WTG_objeto_colorcable1g = new byte[24];
                WTG_objeto_colorcable1g = ReadFully(s, 24);
                string colorcable1g = enc.GetString(WTG_objeto_colorcable1g);

                byte[] WTG_objeto_colorcable1g1 = new byte[16];
                WTG_objeto_colorcable1g1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "colorcable1b"
                byte[] WTG_objeto_colorcable1b = new byte[24];
                WTG_objeto_colorcable1b = ReadFully(s, 24);
                string colorcable1b = enc.GetString(WTG_objeto_colorcable1b);

                byte[] WTG_objeto_colorcable1b1 = new byte[16];
                WTG_objeto_colorcable1b1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "colorcable2r"
                byte[] WTG_objeto_colorcable2r = new byte[24];
                WTG_objeto_colorcable2r = ReadFully(s, 24);
                string colorcable2r = enc.GetString(WTG_objeto_colorcable2r);

                byte[] WTG_objeto_colorcable2r1 = new byte[16];
                WTG_objeto_colorcable2r1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "colorcable2g"
                byte[] WTG_objeto_colorcable2g = new byte[24];
                WTG_objeto_colorcable2g = ReadFully(s, 24);
                string colorcable2g = enc.GetString(WTG_objeto_colorcable2g);

                byte[] WTG_objeto_colorcable2g1 = new byte[16];
                WTG_objeto_colorcable2g1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "colorcable2b"
                byte[] WTG_objeto_colorcable2b = new byte[24];
                WTG_objeto_colorcable2b = ReadFully(s, 24);
                string colorcable2b = enc.GetString(WTG_objeto_colorcable2b);

                byte[] WTG_objeto_colorcable2b1 = new byte[16];
                WTG_objeto_colorcable2b1 = ReadFully(s, 16);

                // Reads data from a stream to find start of  "colorextraidr"
                byte[] WTG_objeto_colorcableIdr = new byte[26];
                WTG_objeto_colorcableIdr = ReadFully(s, 26);
                string colorextraidr = enc.GetString(WTG_objeto_colorcableIdr);

                byte[] WTG_objeto_colorcableIdr1 = new byte[14];
                WTG_objeto_colorcableIdr1 = ReadFully(s, 14);

                // Reads data from a stream to find start of  "colorextraidg"
                byte[] WTG_objeto_colorcableIdg = new byte[26];
                WTG_objeto_colorcableIdg = ReadFully(s, 26);
                string colorextraidg = enc.GetString(WTG_objeto_colorcableIdg);

                byte[] WTG_objeto_colorcableIdg1 = new byte[14];
                WTG_objeto_colorcableIdg1 = ReadFully(s, 14);

                // Reads data from a stream to find start of  "colorextraidb"
                byte[] WTG_objeto_colorcableIdb = new byte[26];
                WTG_objeto_colorcableIdb = ReadFully(s, 26);
                string colorextraidb = enc.GetString(WTG_objeto_colorcableIdb);

                byte[] WTG_objeto_colorcableIdb1 = new byte[14];
                WTG_objeto_colorcableIdb1 = ReadFully(s, 14);

                if (objcount > 9)
                {
                    if (objcount < 100)
                    {
                        byte[] WTG_objeto_colorcableIdb2 = new byte[2];
                        WTG_objeto_colorcableIdb2 = ReadFully(s, 2);
                    }
                    //else
                    //{
                    //    stop = "stop";
                    //}
                }
                //else
                //{
                //    stop = "stop";
                //}

                //////Start Created for finding tag's in WTG file
                ////string pathNew = @"E:\Emdep\00 Ganaraj\Work\Task1\tests\newfile" + objcount.ToString() + ".txt";
                //////Created for Save BMP image separately in WTG file
                //////string pathNew = @"D:\TestWTGtoBMP.bmp";
                ////// Write the byte array to the other FileStream.
                ////using (FileStream fsNew = new FileStream(pathNew, FileMode.Create, FileAccess.Write))
                ////{
                ////    //fsNew.Write(Fix_Byte, 0, Fix_Byte.Length);
                ////    //fsNew.Write(File_Size, 0, File_Size.Length);
                ////    //fsNew.Write(pic_property3, 0, pic_property3.Length);
                ////    //fsNew.Write(pic_property4, 0, pic_property4.Length);
                ////    //fsNew.Write(BM_Head0, 0, BM_Head0.Length);
                ////    //fsNew.Write(BM_Head1, 0, BM_Head6.Length);
                ////    //fsNew.Write(BM_Head2, 0, BM_Head6.Length);
                ////    //fsNew.Write(BM_Head3, 0, BM_Head6.Length);
                ////    //fsNew.Write(BM_Head4, 0, BM_Head6.Length);
                ////    //fsNew.Write(BM_Head5, 0, BM_Head6.Length);
                ////    //fsNew.Write(BM_Head6, 0, BM_Head6.Length);

                ////    //fsNew.Write(BM_Head, 0, BM_Head.Length);
                ////    //fsNew.Write(WTG_nObject, 0, WTG_nObject.Length);
                ////    //fsNew.Write(WTG_nObject_count, 0, WTG_nObject_count.Length);
                ////    //fsNew.Write(WTG_identlable, 0, WTG_identlable.Length);
                ////    //fsNew.Write(WTG_identlable, 0, WTG_identlable.Length);
                ////    fsNew.Write(WTG_objeto, 0, WTG_objeto.Length);
                ////    fsNew.Write(WTG_objeto_destino, 0, WTG_objeto_destino.Length);
                ////    fsNew.Write(WTG_objeto_destino_x, 0, WTG_objeto_destino_x.Length);
                ////    fsNew.Write(WTG_objeto_destino_y, 0, WTG_objeto_destino_y.Length);
                ////    //fsNew.Write(WTG_objeto_estaseleccionado, 0, WTG_objeto_estaseleccionado.Length);
                ////    //fsNew.Write(WTG_objeto_extraidentificado, 0, WTG_objeto_extraidentificado.Length);
                ////    fsNew.Write(WTG_objeto_labelcaption, 0, WTG_objeto_labelcaption.Length);
                ////    //fsNew.Write(WTG_objeto_labelname, 0, WTG_objeto_labelname.Length);
                ////    //fsNew.Write(WTG_objeto_m_label1, 0, WTG_objeto_m_label1.Length);
                ////    //fsNew.Write(WTG_objeto_m_label1_labelheight, 0, WTG_objeto_m_label1_labelheight.Length);
                ////    //fsNew.Write(WTG_objeto_m_label1_labelwidth, 0, WTG_objeto_m_label1_labelwidth.Length);
                ////    //fsNew.Write(WTG_objeto_isblinking, 0, WTG_objeto_isblinking.Length);
                ////    //fsNew.Write(WTG_objeto_moviendose, 0, WTG_objeto_moviendose.Length);
                ////    fsNew.Write(WTG_objeto_origen, 0, WTG_objeto_origen.Length);
                ////    //fsNew.Write(WTG_objeto_origen1_x0, 0, WTG_objeto_origen1_x0.Length);
                ////    //fsNew.Write(WTG_objeto_origen1_x, 0, WTG_objeto_origen1_x.Length);
                ////    //fsNew.Write(WTG_objeto_origen1_y, 0, WTG_objeto_origen1_y.Length);
                ////    fsNew.Write(WTG_objeto_radio, 0, WTG_objeto_radio.Length);
                ////    //fsNew.Write(WTG_objeto_redimensionando, 0, WTG_objeto_redimensionando.Length);
                ////    //fsNew.Write(WTG_objeto_relleno, 0, WTG_objeto_relleno.Length);
                ////    //fsNew.Write(WTG_objeto_rellenoAux, 0, WTG_objeto_rellenoAux.Length);
                ////    fsNew.Write(WTG_objeto_tipoobjeto, 0, WTG_objeto_tipoobjeto.Length);
                ////    //fsNew.Write(WTG_objeto_visible, 0, WTG_objeto_visible.Length);
                ////    //fsNew.Write(WTG_objeto_pintocables, 0, WTG_objeto_pintocables.Length);
                ////    //fsNew.Write(WTG_objeto_numorden, 0, WTG_objeto_numorden.Length);
                ////    fsNew.Write(WTG_objeto_tipopieza, 0, WTG_objeto_tipopieza.Length);
                ////    //fsNew.Write(WTG_objeto_r, 0, WTG_objeto_r.Length);
                ////    //fsNew.Write(WTG_objeto_g, 0, WTG_objeto_g.Length);
                ////    //fsNew.Write(WTG_objeto_bl, 0, WTG_objeto_bl.Length);
                ////    //fsNew.Write(WTG_objeto_colorcable1r, 0, WTG_objeto_colorcable1r.Length);
                ////    //fsNew.Write(WTG_objeto_colorcable1g, 0, WTG_objeto_colorcable1g.Length);
                ////    //fsNew.Write(WTG_objeto_colorcable1b, 0, WTG_objeto_colorcable1b.Length);
                ////    //fsNew.Write(WTG_objeto_colorcableIdr, 0, WTG_objeto_colorcableIdr.Length);
                ////    //fsNew.Write(WTG_objeto_colorcableIdg, 0, WTG_objeto_colorcableIdg.Length);
                ////    //fsNew.Write(WTG_objeto_colorcableIdb, 0, WTG_objeto_colorcableIdb.Length);

                ////    //fsNew.Write(WTG_objeto_m_label1, 0, WTG_objeto_m_label1.Length);
                ////    //fsNew.Write(WTG_identlable2, 0, WTG_identlable2.Length);
                ////    //fsNew.Write(WTG_identlable3, 0, WTG_identlable3.Length);
                ////    //fsNew.Write(WTG_identlable4, 0, WTG_identlable4.Length);
                ////    //fsNew.Write(WTG_identlable5, 0, WTG_identlable5.Length);
                ////    //fsNew.Write(WTG_identlable6, 0, WTG_identlable6.Length);
                ////    //fsNew.Write(WTG_identlable7, 0, WTG_identlable7.Length);
                ////    //imageLoad = new Bitmap(fsNew);
                ////    //Bitmap WTG_Image = new Bitmap(fsNew);
                ////    //imageLoad = new Bitmap(fsNew);
                ////}
                //////End Created for finding tag's in WTG file

                //objcount--;         //Decrement count and serch for next object values
                objcount++;
            }

            s.Close();
            imageLoad = new Bitmap(s_Image);


            //Bitmap myBitmap = new Bitmap(s_Image);
            ////WTGValues.MyPic = st myBitmap;
            //stdole.IPictureDisp iPic;
            //(stdole.IPictureDisp)GetIPictureDispFromPicture(imageLoad);

            //stdole.lo
            //iPic = myBitmap;

            //Draw ways and detection on BMP Image
            objcount = 0;
            WTGValues.LabelArrayList = new Dictionary<string, Label>();
            WTGValues.LabelLocation = new Dictionary<string, PointF>();
            WTGValues.ObjectArrayList = new Dictionary<string, Panel>();
            WTGValues.ObjectLocation = new Dictionary<string, PointF>();
            WTGValues.Shapedetails = new Dictionary<string, Panel>();

            while (objcount != nObject_count)
            {
                if (Convert.ToBoolean(al_obj_visible[objcount]))
                {
                    //Graphics xGraph;

                    int r = Convert.ToInt32(al_obj_rdo[objcount]); // radius of circle
                    float x, y; // center coordinates of circle
                    x = Convert.ToSingle(al_obj_orgn_x[objcount]);
                    y = Convert.ToSingle(al_obj_orgn_y[objcount]);
                    Random rnd = new Random((int)DateTime.Now.Ticks); // seeded with ticks
                    Pen myPen = new Pen(Color.Red);
                    myPen.Color = Color.FromArgb(255, 20, 147);

                    Pen myPenFill = new Pen(Color.Red);
                    myPenFill.Color = Color.FromArgb(Convert.ToInt32(al_obj_r[objcount]), Convert.ToInt32(al_obj_g[objcount]), Convert.ToInt32(al_obj_bl[objcount]));
                    SolidBrush myBrashFill = new SolidBrush(Color.FromArgb(Convert.ToInt32(al_obj_r[objcount]), Convert.ToInt32(al_obj_g[objcount]), Convert.ToInt32(al_obj_bl[objcount])));
                    //Color clr = new Color();


                    //xGraph = Graphics.FromImage(imageLoad);
                    //xGraph.SmoothingMode = SmoothingMode.AntiAlias;
                    //xGraph.TextRenderingHint = TextRenderingHint.AntiAlias;
                    Font stringFont = new Font("Arial", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
                    // Measure string. //Microsoft Sans Serif, Arial
                    SizeF stringSize = new SizeF();
                    //stringSize = xGraph.MeasureString(al_obj_labelcaption[objcount].ToString(), stringFont);

                    Label lb = new Label();
                    ToolTip tltp = new ToolTip();
                    lb.AutoSize = true;
                    lb.BackColor = System.Drawing.SystemColors.HighlightText;       //System.Drawing.Color.Transparent;
                    lb.Font = stringFont;
                    //lb.Location = new System.Drawing.Point(50, 50);     // start point of label
                    lb.Name = "labelNew";       //m_lablel<n>
                    lb.Size = new System.Drawing.Size(Convert.ToInt32(stringSize.Width), Convert.ToInt32(stringSize.Height));
                    //lb.TabIndex = 8;
                    lb.Text = al_obj_labelcaption[objcount].ToString().Replace("\0", "");       //labelcaption
                    tltp.SetToolTip(lb, al_obj_labelcaption[objcount].ToString().Replace("\0", ""));

                    Panel pnl = new Panel();
                    pnl.Name = WTGValues.ObjectName[objcount].ToString();
                    Label lbl = new Label();
                    lbl.Name = "m_label";
                    lbl.BringToFront();
                    lbl.Text = al_obj_labelcaption[objcount].ToString().Replace("\0", "");       //labelcaption
                    tltp.SetToolTip(lbl, al_obj_labelcaption[objcount].ToString().Replace("\0", ""));

                    lbl.BackColor = Color.White;

                    lbl.AutoSize = true;

                    pnl.BackColor = Color.Transparent;

                    Point ptLoc = new Point();
                    Size sz = new Size();

                    switch (Convert.ToInt32(al_obj_tipoobjeto[objcount]))
                    {
                        case 1://   1 is circle
                            if (Convert.ToInt32(al_obj_r[objcount]) != 207 && Convert.ToInt32(al_obj_g[objcount]) != 207 && Convert.ToInt32(al_obj_bl[objcount]) != 207)
                            {
                                //xGraph.FillEllipse(myBrashFill, x - (r - 3), y - (r - 3), (r - 3) * 2, (r - 3) * 2);
                            }
                            //xGraph.FillRectangle(Brushes.GreenYellow, x, y, 3, 3);
                            ////xGraph.DrawRectangle(myPen, Convert.ToSingle(al_obj_dstn_x[objcount]) - 4, Convert.ToSingle(al_obj_dstn_y[objcount]) - 4, 4, 4);
                            //xGraph.DrawEllipse(myPen, x - r, y - r, r * 2, r * 2);

                            ptLoc = new Point(Convert.ToInt32(x - r), Convert.ToInt32(y - r));
                            sz = new Size(Convert.ToInt32(r * 2), Convert.ToInt32(r * 2));

                            //Draw String on image to display labelcaption
                            //xGraph.FillRectangle(Brushes.White, x + 1, y + 1, stringSize.Width, stringSize.Height);    //(al_obj_labelcaption[objcount].ToString().Length * 3) + 0, 8);
                            //xGraph.DrawString(al_obj_labelcaption[objcount].ToString(), stringFont, new SolidBrush(Color.Black), new PointF(x + 2, y + 2));

                            // Label caption Location
                            lb.Location = new System.Drawing.Point(Convert.ToInt32(x + 2), Convert.ToInt32(y + 2));     // start point of label
                            break;
                        case 0://   0 is rectangle
                            float temp_diff;
                            if (Convert.ToSingle(al_obj_dstn_x[objcount]) < x)
                            {
                                if (Convert.ToSingle(al_obj_dstn_y[objcount]) < y)
                                {
                                    //xGraph.FillRectangle(Brushes.GreenYellow, x - 3, y - 3, 3, 3);
                                    ////xGraph.DrawRectangle(myPen, Convert.ToSingle(al_obj_dstn_x[objcount]), Convert.ToSingle(al_obj_dstn_y[objcount]), 4, 4);

                                    float temp;
                                    temp = x;
                                    x = Convert.ToSingle(al_obj_dstn_x[objcount]);
                                    al_obj_dstn_x[objcount] = temp;

                                    temp = y;
                                    y = Convert.ToSingle(al_obj_dstn_y[objcount]);
                                    al_obj_dstn_y[objcount] = temp;
                                }
                                else
                                {
                                    //xGraph.FillRectangle(Brushes.GreenYellow, x - 3, y, 3, 3);
                                    ////xGraph.DrawRectangle(myPen, Convert.ToSingle(al_obj_dstn_x[objcount]), Convert.ToSingle(al_obj_dstn_y[objcount]) - 4, 4, 4);

                                    temp_diff = Math.Abs(Convert.ToSingle(al_obj_dstn_x[objcount]) - x);
                                    al_obj_dstn_x[objcount] = (temp_diff + Convert.ToSingle(al_obj_dstn_x[objcount]));
                                    x = (x - temp_diff);
                                }
                            }
                            else
                            {
                                if (Convert.ToSingle(al_obj_dstn_y[objcount]) < y)
                                {
                                    //xGraph.FillRectangle(Brushes.GreenYellow, x, y - 4, 3, 3);
                                    ////xGraph.DrawRectangle(myPen, Convert.ToSingle(al_obj_dstn_x[objcount]) - 4, Convert.ToSingle(al_obj_dstn_y[objcount]), 4, 4);

                                    temp_diff = Math.Abs(Convert.ToSingle(al_obj_dstn_y[objcount]) - y);
                                    al_obj_dstn_y[objcount] = (temp_diff + Convert.ToSingle(al_obj_dstn_y[objcount]));
                                    y = (y - temp_diff);
                                }
                                else
                                {
                                    //xGraph.FillRectangle(Brushes.GreenYellow, x, y, 3, 3);
                                    ////xGraph.DrawRectangle(myPen, Convert.ToSingle(al_obj_dstn_x[objcount]) - 4, Convert.ToSingle(al_obj_dstn_y[objcount]) - 4, 4, 4);
                                }
                            }
                            if (Convert.ToInt32(al_obj_r[objcount]) != 207 && Convert.ToInt32(al_obj_g[objcount]) != 207 && Convert.ToInt32(al_obj_bl[objcount]) != 207)
                            {
                                //xGraph.FillRectangle(myBrashFill, x + 3, y + 3, Convert.ToSingle(al_obj_dstn_x[objcount]) - x - 6, Convert.ToSingle(al_obj_dstn_y[objcount]) - y - 6);
                            }

                            //Draw Rectangle
                            //xGraph.DrawRectangle(myPen, x, y, Convert.ToSingle(al_obj_dstn_x[objcount]) - x, Convert.ToSingle(al_obj_dstn_y[objcount]) - y);

                            ptLoc = new Point(Convert.ToInt32(x), Convert.ToInt32(y));
                            sz = new Size(Convert.ToInt32(Convert.ToSingle(al_obj_dstn_x[objcount]) - x), Convert.ToInt32(Convert.ToSingle(al_obj_dstn_y[objcount]) - y));

                            //Draw String on image to display labelcaption
                            ////xGraph.FillRectangle(Brushes.White, (x + ((Convert.ToSingle(al_obj_dstn_x[objcount]) - x) / 2)) + 1, (y + ((Convert.ToSingle(al_obj_dstn_y[objcount]) - y) / 2)) + 1, stringSize.Width, stringSize.Height); //(al_obj_labelcaption[objcount].ToString().Length * 3), 8);
                            ////xGraph.DrawString(al_obj_labelcaption[objcount].ToString(), stringFont, new SolidBrush(Color.Black), new PointF(x + ((Convert.ToSingle(al_obj_dstn_x[objcount]) - x) / 2) + 2, y + ((Convert.ToSingle(al_obj_dstn_y[objcount]) - y) / 2) + 2));

                            // Label caption Location
                            lb.Location = new System.Drawing.Point(Convert.ToInt32(x + ((Convert.ToSingle(al_obj_dstn_x[objcount]) - x) / 2) + 2), Convert.ToInt32(y + ((Convert.ToSingle(al_obj_dstn_y[objcount]) - y) / 2) + 2));
                            break;
                        default:
                            //do nothing
                            break;
                    }
                    WTGValues.LabelArrayList.Add(WTGValues.ObjectName[objcount].ToString(), lb);
                    WTGValues.LabelLocation.Add(WTGValues.ObjectName[objcount].ToString(), lb.Location);

                    pnl.Location = ptLoc;
                    pnl.Size = sz;

                    lbl.Location = new Point(sz.Width / 2, sz.Height / 2);

                    pnl.Controls.Add(lbl);

                    WTGValues.Shapedetails.Add(WTGValues.ObjectName[objcount].ToString(), pnl);
                    WTGValues.ObjectArrayList.Add(WTGValues.ObjectName[objcount].ToString(), pnl);
                    //xGraph.Dispose();
                }
                else
                {
                    Panel pnl = new Panel();
                    Label lb = new Label();
                    pnl.Name = WTGValues.ObjectName[objcount].ToString();
                    WTGValues.LabelArrayList.Add(WTGValues.ObjectName[objcount].ToString(), lb);
                    WTGValues.ObjectArrayList.Add(WTGValues.ObjectName[objcount].ToString(), pnl);

                }
                objcount++;         //Increment count and serch for next object to insert in image
            }

            WTGValues.Pic = imageLoad;
            return imageLoad;
        }

        /// <summary>
        /// Read stream from current position to particular lenght.
        /// </summary>
        /// <param name="stream"> Stream to pass from which byte array read.</param>
        /// <param name="length"> lenght of the byte array required.</param>
        /// <returns>Returns <paramref name="ms"/> value i.e. byte array.</returns>
        /// <remarks>
        /// Extract value from stream current position upto lenght you want and it returns n-byte's array.
        /// </remarks>
        /// <example>
        /// <code>
        /// byte[] barray = new byte[4];
        /// barray = ReadFully(s, 4);
        /// </code>
        /// </example>
        public static byte[] ReadFully(Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            long offset = stream.Position;
            using (MemoryStream ms = new MemoryStream())
            {
                int read = stream.Read(buffer, 0, length);
                ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }
    }
}
