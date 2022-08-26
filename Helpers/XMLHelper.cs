using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ManageTreeDemo.Model;
using ManageTreeDemo.Common;
using System.Reflection;
using System.Windows;

namespace ManageTreeDemo.Helpers
{
    public class XmlHelper
    {
        #region 构造函数
        public XmlHelper()
        {

        }
        #endregion

        #region BaseFucs
        #region 创建XMLDocument
        /// <summary>
        /// 创建XMLDocument
        /// </summary>
        /// <param name="name">根节点名称</param>
        /// <param name="type">根节点的一个属性值</param>
        /// <returns></returns>
        /// .net中调用方法：写入文件中,则：
        /// document = XmlOperate.CreateXmlDocument("sex", "sexy");
        /// document.Save("c:/bookstore.xml");         
        public static XmlDocument CreateXmlDocument(string name, string type)
        {
            XmlDocument doc = null;
            XmlElement rootEle = null;

            try
            {
                doc = new XmlDocument();
                doc.LoadXml("<" + name + "/>");
                rootEle = doc.DocumentElement;
                rootEle.SetAttribute("type", type);
            }
            catch (Exception ex)
            {
                throw new Exception("", ex) ;
            }
            return doc;
        }
        #endregion

        #region 枚举定义
        public enum XmlType
        {
            File,
            String
        };
        #endregion

        #endregion

        #region 插入节点

        #region 递归插入节点(粘贴）
        public static bool InsertByRecursion<T>(string xmlpath,string site, T data) where T : Node, new()
        {
            try 
            {
                //xml节点路径相同（同位置同名节点，添加属性只能找到第一个节点）
                //添加节点
                Insert(xmlpath, site, data.NodeName, "Name", data.NodeName);
                //遍历对象属性
                PropertyInfo[] PropertyInfos = data.GetType().GetProperties();
                foreach (var property in PropertyInfos)
                {
                    object obj = property.GetValue(data);
                }
                //插入属性
                Insert(xmlpath, site + "/" + data.NodeName, "", "NodeType", data.NodeType.ToString()) ;
                //Insert(xmlpath, site + "/" + data.NodeName, "", "NodeContent", data.NodeContent);
                foreach (T childnode in data.ChildNodes)
                {
                    InsertByRecursion<T>(xmlpath, site + "/" + data.NodeName, childnode);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
        #region 插入节点（元素名属性名属性值）
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">插入值父节点（节点路径）</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/root", "Element", "", "Value")
         * XmlHelper.Insert(path, "/root/lev1", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/root/lev1/lev2", "", "Attribute", "Value")
         ************************************************/
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message); 
            }
        }
        #endregion
        #region insert'
        /// <summary>
        /// 插入数据(元素名等于属性名)
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
         ************************************************/
        public static void Insert(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (value.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(value);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(path);
                //MessageBox.Show("插入成功");
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
                //MessageBox.Show(e.Message);
            }
        }
        #endregion
        #endregion

        #region 修改节点
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Update(path, "/Node", "", "Value")
         * XmlHelper.Update(path, "/Node", "Attribute", "Value")
         ************************************************/
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(path);
            }
            catch { }
        }
        #endregion

        #region 删除节点
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Delete(path, "/Node", "")
         * XmlHelper.Delete(path, "/Node", "Attribute")
         ************************************************/
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(path);
                
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion

        #region 操作xml文件中指定节点的数据
        /// <summary>
        /// 获得xml文件中指定节点的节点数据
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string GetNodeInfoByNodeName(string path, string nodeName)
        {
            string XmlString = "";
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            System.Xml.XmlElement root = xml.DocumentElement;
            System.Xml.XmlNode node = root.SelectSingleNode("//" + nodeName);
            if (node != null)
            {
                XmlString = node.InnerText;
            }
            return XmlString;
        }

        #endregion

        #region 读取XML资源中的指定节点内容
        /// <summary>
        /// 读取XML资源中的指定节点内容
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>节点内容</returns>
        public static object GetNodeValue(string source, XmlType xmlType, string nodeName)
        {
            XmlDocument xd = new XmlDocument();
            if (xmlType == XmlType.File)
            {
                xd.Load(source);
            }
            else
            {
                xd.LoadXml(source);
            }
            XmlElement xe = xd.DocumentElement;
            XmlNode xn = xe.SelectSingleNode("//" + nodeName);
            if (xn != null)
            {
                return xn.InnerText;
            }
            else
            {
                return null;
            }
        }

        


        /// <summary>
        /// 读取XML资源中的指定节点内容
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>节点内容</returns>
        public static object GetNodeValue(string source, string nodeName)
        {
            if (source == null || nodeName == null || source == "" || nodeName == "" || source.Length < nodeName.Length * 2)
            {
                return null;
            }
            else
            {
                int start = source.IndexOf("<" + nodeName + ">") + nodeName.Length + 2;
                int end = source.IndexOf("</" + nodeName + ">");
                if (start == -1 || end == -1)
                {
                    return null;
                }
                else if (start >= end)
                {
                    return null;
                }
                else
                {
                    return source.Substring(start, end - start);
                }
            }
        }
        #endregion

        #region 递归读取xml树
        #region 递归读取xml树1
        /// <summary>
        /// 递归读取Xml树返回树结构对象
        /// </summary>
        /// <typeparam name="T">返回实体，约束：无参构造函数，string TName成员，List<T>同类型子集</typeparam>
        /// <param name="xmlpath">xml文件绝对路径</param>
        /// <param name="xmlsite">读取根节点 eg:"root/lev1/lev2","root"</param>
        /// <returns>指定类型</returns>
        public static T GetXmlTreeByRecursion<T> (string xmlpath, string xmlsite) where T:Node,new()
        {
            T node = new T();
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略xml中注释
            XmlReader reader = XmlReader.Create(xmlpath, settings);
            doc.Load(reader);
            XmlNode xmlnode = doc.SelectSingleNode(xmlsite);
            XmlElement xmlElement = (XmlElement)xmlnode;
            ///赋值节点信息
            node.NodeName = xmlElement.GetAttribute("Name").ToString();
            node.Site = xmlsite;
            foreach (XmlNode childnode in xmlnode)
            {
                XmlElement childnodeElement = (XmlElement)childnode;
                node.ChildNodes.Add(GetXmlTreeByRecursion<T>(xmlpath, xmlsite + "/" + childnodeElement.GetAttribute("Name").ToString()));
            }
            return node;
        }
        #endregion

        #region 递归读取xml树2
        /// <summary>
        /// 递归读取Xml树返回树结构对象
        /// </summary>
        /// <typeparam name="T">返回实体，约束：无参构造函数，string TName成员，List<T>同类型子集</typeparam>
        /// <param name="xmlpath">xml文件绝对路径</param>
        /// <param name="xmlsite">目标根节点xml标签名 eg:"root/lev1/lev2","root"</param>
        /// <returns>指定类型</returns>
        /// 重载函数
        public static T GetXmlTreeByRecursion<T>(string xmlpath, string xmlsite,bool _same) where T : Node, new()
        {
            T node = new T();
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略xml中注释
            XmlReader reader = XmlReader.Create(xmlpath, settings);
            doc.Load(reader);
            XmlNode xmlnode = doc.SelectSingleNode(xmlsite);
            reader.Close();
            XmlElement xmlElement = (XmlElement)xmlnode;
            ///赋值节点信息
            node.NodeName = xmlElement.GetAttribute("Name").ToString();
            node.Site = xmlsite;
            node.NodeType =(NodeType)System.Enum.Parse(typeof(NodeType), xmlElement.GetAttribute("NodeType").ToString());
            //node.NodeContent = xmlElement.GetAttribute("NodeContent").ToString();
            foreach (XmlNode childnode in xmlnode)
            {
                XmlElement childnodeElement = (XmlElement)childnode;
                //node.ChildNodes.Add(getxmlNodes(xmlpath, node.Site+"/" +childnodeElement.GetAttribute("Name" ).ToString()));
                if (childnode.Attributes["NodeType"].Value.ToString() == NodeType.Normal.ToString())
                    node.ChildNodes.Add(GetXmlTreeByRecursion<Node>(xmlpath, node.Site + "/" + childnode.Name.ToString(), false));
                else
                    node.ChildNodes.Add(GetXmlTreeByRecursion<Model.Directory>(xmlpath, node.Site + "/" + childnode.Name.ToString(), false));
            }
            foreach (var childnode in node.ChildNodes)
            {
                childnode.ParentNode = node;
            }
            return node;
        }
        #endregion
        #endregion

        #region 更新XML文件中的指定节点内容
        /// <summary>
        /// 更新XML文件中的指定节点内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">更新内容</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateNode(string filePath, string nodeName, string nodeValue)
        {
            bool flag = false;
            XmlDocument xd = new XmlDocument();
            xd.Load(filePath);
            XmlElement xe = xd.DocumentElement;
            XmlNode xn = xe.SelectSingleNode("//" + nodeName);

            if (xn != null)
            {
                xn.InnerText = nodeValue;
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        #endregion

        #region 读取xml文件，并将文件序列化为类
        /// <summary>
        /// 读取xml文件，并将文件序列化为类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T ReadXML<T>(string path)
        {
            XmlSerializer reader = new XmlSerializer(typeof(T));//TODO
            StreamReader file = new StreamReader(@path);
            return (T)reader.Deserialize(file);
        }
        #endregion

        #region 将对象写入XML文件
        /// <summary>
        /// 将对象写入XML文件
        /// </summary>
        /// <typeparam name="T">C#对象名</typeparam>
        /// <param name="item">对象实例</param>
        /// <param name="path">路径</param>
        /// <param name="jjdbh">标号</param>
        /// <param name="ends">结束符号（整个xml的路径类似如下：C:\xmltest\201111send.xml，其中path=C:\xmltest,jjdbh=201111,ends=send）</param>
        /// <returns></returns>
        public static string WriteXML<T>(T item, string path, string jjdbh, string ends)
        {
            if (string.IsNullOrEmpty(ends))
            {
                //默认为发送
                ends = "send";
            }
            int i = 0;//控制写入文件的次数，
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            object[] obj = new object[] { path, "\\", jjdbh, ends, ".xml" };
            string xmlPath = String.Concat(obj);
            while (true)
            {
                try
                {
                    //用filestream方式创建文件不会出现“文件正在占用中，用File.create”则不行
                    FileStream fs;
                    fs = File.Create(xmlPath);
                    fs.Close();
                    TextWriter writer = new StreamWriter(xmlPath, false, Encoding.UTF8);
                    XmlSerializerNamespaces xml = new XmlSerializerNamespaces();
                    xml.Add(string.Empty, string.Empty);
                    serializer.Serialize(writer, item, xml);
                    writer.Flush();
                    writer.Close();
                    break;
                }
                catch (Exception)
                {
                    if (i < 5)
                    {
                        i++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return SerializeToXmlStr<T>(item, true);
        }
        #endregion

        #region 静态扩展
        /// <summary>
        /// 静态扩展
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型，必须声明[Serializable]特征</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="omitXmlDeclaration">true:省略XML声明;否则为false.默认false，即编写 XML 声明。</param>
        /// <returns></returns>
        public static string SerializeToXmlStr<T>(T obj, bool omitXmlDeclaration)
        {
            return XmlSerialize<T>(obj, omitXmlDeclaration);
        }
        #endregion

        #region XML序列化反序列化相关的静态方法
        /// <summary>
        /// 使用XmlSerializer序列化对象
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型，必须声明[Serializable]特征</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="omitXmlDeclaration">true:省略XML声明;否则为false.默认false，即编写 XML 声明。</param>
        /// <returns>序列化后的字符串</returns>
        public static string XmlSerialize<T>(T obj, bool omitXmlDeclaration)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.OmitXmlDeclaration = omitXmlDeclaration;
            xmlSettings.Encoding = new System.Text.UTF8Encoding(false);
            MemoryStream stream = new MemoryStream();//var writer = new StringWriter();
            //这里如果直接写成：Encoding = Encoding.UTF8 会在生成的xml中加入BOM(Byte-order Mark) 信息(Unicode 字节顺序标记) ， 
            //所以new System.Text.UTF8Encoding(false)是最佳方式，省得再做替换的麻烦
            XmlWriter xmlwriter = XmlWriter.Create(stream/*writer*/, xmlSettings);
            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add(String.Empty, String.Empty); //在XML序列化时去除默认命名空间xmlns:xsd和xmlns:xsi
            XmlSerializer ser = new XmlSerializer(typeof(T));
            ser.Serialize(xmlwriter, obj, xmlns);
            return Encoding.UTF8.GetString(stream.ToArray());//writer.ToString();
        }



        /// <summary>
        /// 使用XmlSerializer序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="omitXmlDeclaration">true:省略XML声明;否则为false.默认false，即编写 XML 声明。</param>
        /// <param name="removeDefaultNamespace">是否移除默认名称空间(如果对象定义时指定了:XmlRoot(Namespace = "http://www.xxx.com/xsd")则需要传false值进来)</param>
        /// <returns>序列化后的字符串</returns>
        public static void XmlSerialize<T>(string path, T obj, bool omitXmlDeclaration, bool removeDefaultNamespace)
        {
            XmlWriterSettings xmlSetings = new XmlWriterSettings();
            xmlSetings.OmitXmlDeclaration = omitXmlDeclaration;
            using (XmlWriter xmlwriter = XmlWriter.Create(path, xmlSetings))
            {
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                if (removeDefaultNamespace)
                    xmlns.Add(String.Empty, String.Empty); //在XML序列化时去除默认命名空间xmlns:xsd和xmlns:xsi
                XmlSerializer ser = new XmlSerializer(typeof(T));
                ser.Serialize(xmlwriter, obj, xmlns);
            }
        }

        private static byte[] ShareReadFile(string filePath)
        {
            byte[] bytes;
            //避免"正由另一进程使用,因此该进程无法访问此文件"造成异常 共享锁 flieShare必须为ReadWrite，
            //但是如果文件不存在的话，还是会出现异常，所以这里不能吃掉任何异常，但是需要考虑到这些问题 
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bytes = new byte[fs.Length];
                int numBytesToRead = (int)fs.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    int n = fs.Read(bytes, numBytesRead, numBytesToRead);
                    if (n == 0)
                        break;
                    numBytesRead += n;
                    numBytesToRead -= n;
                }
            }
            return bytes;
        }

        /// <summary>
        /// 从文件读取并反序列化为对象 （解决: 多线程或多进程下读写并发问题）
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="path">文件地址</param>
        /// <returns></returns>
        public static T XmlFileDeserialize<T>(string path)
        {
            byte[] bytes = ShareReadFile(path);
            //当文件正在被写入数据时，可能读出为0
            if (bytes.Length < 1)
            {
                //5次机会
                for (int i = 0; i < 5; i++)
                {
                    // 采用这样诡异的做法避免独占文件和文件正在被写入时读出来的数据为0字节的问题。
                    bytes = ShareReadFile(path);
                    if (bytes.Length > 0) break;
                    //悲观情况下总共最多消耗1/4秒，读取文件
                    System.Threading.Thread.Sleep(50);
                }
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(new MemoryStream(bytes));
            if (doc.DocumentElement != null)
                return (T)new XmlSerializer(typeof(T)).Deserialize(new XmlNodeReader(doc.DocumentElement));
            return default(T);

            //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            //xmlReaderSettings.CloseInput = true;
            //using (XmlReader xmlReader = XmlReader.Create(path, xmlReaderSettings))
            //{
            //    T obj = (T)new XmlSerializer(typeof(T)).Deserialize(xmlReader);
            //    return obj;
            //}
        }



        /// <summary>
        /// 使用XmlSerializer反序列化对象
        /// </summary>
        /// <param name="xmlOfObject">需要反序列化的xml字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T XmlDeserialize<T>(string xmlOfObject) where T : class
        {
            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlOfObject), new XmlReaderSettings());
            return (T)new XmlSerializer(typeof(T)).Deserialize(xmlReader);
        }

        #endregion

        #region DataSet/DataTable相关
        #region 获取一个字符串xml文档中的dataset
        /// <summary>
        /// 获取一个字符串xml文档中的dataset
        /// </summary>
        /// <param name="xml_string">含有xml信息的字符串</param>
        public static void get_XmlValue_ds(string xml_string, ref DataSet ds)
        {
            System.Xml.XmlDocument xd = new XmlDocument();
            xd.LoadXml(xml_string);
            XmlNodeReader xnr = new XmlNodeReader(xd);
            ds.ReadXml(xnr);
            xnr.Close();
            int a = ds.Tables.Count;
        }

        #endregion

        #region 读取XML资源到DataSet中
        /// <summary>
        /// 读取XML资源到DataSet中
        /// </summary>
        /// <param name="source">XML资源，文件为路径，否则为XML字符串</param>
        /// <param name="xmlType">XML资源类型</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string source, XmlType xmlType)
        {
            DataSet ds = new DataSet();
            if (xmlType == XmlType.File)
            {
                ds.ReadXml(source);
            }
            else
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xd);
                ds.ReadXml(xnr);
            }
            return ds;
        }
        #endregion

        #region 读取XML资源到DataTable中
        /// <summary>
        /// 读取XML资源到DataTable中
        /// </summary>
        /// <param name="source">XML资源，文件为路径，否则为XML字符串</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="tableName">表名称</param>
        /// <returns>DataTable</returns>
        public static DataTable GetTable(string source, XmlType xmlType, string tableName)
        {
            DataSet ds = new DataSet();
            if (xmlType == XmlType.File)
            {
                ds.ReadXml(source);
            }
            else
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xd);
                ds.ReadXml(xnr);
            }
            return ds.Tables[tableName];
        }

        #endregion

        #region 读取XML资源中指定的DataTable的指定行指定列的值
        /// <summary>
        /// 读取XML资源中指定的DataTable的指定行指定列的值
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colName">列名</param>
        /// <returns>值，不存在时返回Null</returns>
        public static object GetTableCell(string source, XmlType xmlType, string tableName, int rowIndex, string colName)
        {
            DataSet ds = new DataSet();
            if (xmlType == XmlType.File)
            {
                ds.ReadXml(source);
            }
            else
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xd);
                ds.ReadXml(xnr);
            }
            return ds.Tables[tableName].Rows[rowIndex][colName];
        }

        #endregion

        #region 读取XML资源中指定的DataTable的指定行指定列的值
        /// <summary>
        /// 读取XML资源中指定的DataTable的指定行指定列的值
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colIndex">列号</param>
        /// <returns>值，不存在时返回Null</returns>
        public static object GetTableCell(string source, XmlType xmlType, string tableName, int rowIndex, int colIndex)
        {
            DataSet ds = new DataSet();
            if (xmlType == XmlType.File)
            {
                ds.ReadXml(source);
            }
            else
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xd);
                ds.ReadXml(xnr);
            }
            return ds.Tables[tableName].Rows[rowIndex][colIndex];
        }

        #endregion

        #region 将DataTable写入XML文件中
        /// <summary>
        /// 将DataTable写入XML文件中
        /// </summary>
        /// <param name="dt">含有数据的DataTable</param>
        /// <param name="filePath">文件路径</param>
        public static void SaveTableToFile(DataTable dt, string filePath)
        {
            DataSet ds = new DataSet("Config");
            ds.Tables.Add(dt.Copy());
            ds.WriteXml(filePath);
        }

        #endregion

        #region 将DataTable以指定的根结点名称写入文件
        /// <summary>
        /// 将DataTable以指定的根结点名称写入文件
        /// </summary>
        /// <param name="dt">含有数据的DataTable</param>
        /// <param name="rootName">根结点名称</param>
        /// <param name="filePath">文件路径</param>
        public static void SaveTableToFile(DataTable dt, string rootName, string filePath)
        {
            DataSet ds = new DataSet(rootName);
            ds.Tables.Add(dt.Copy());
            ds.WriteXml(filePath);
        }

        #endregion

        #region 使用DataSet方式更新XML文件节点

        /// <summary>
        /// 使用DataSet方式更新XML文件节点
        /// </summary>
        /// <param name="filePath">XML文件路径</param>
        /// <param name="tableName">表名称</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colName">列名</param>
        /// <param name="content">更新值</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateTableCell(string filePath, string tableName, int rowIndex, string colName, string content)
        {
            bool flag = false;
            DataSet ds = new DataSet();
            ds.ReadXml(filePath);
            DataTable dt = ds.Tables[tableName];

            if (dt.Rows[rowIndex][colName] != null)
            {
                dt.Rows[rowIndex][colName] = content;
                ds.WriteXml(filePath);
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        #endregion

        #region 使用DataSet方式更新XML文件节点
        /// <summary>
        /// 使用DataSet方式更新XML文件节点
        /// </summary>
        /// <param name="filePath">XML文件路径</param>
        /// <param name="tableName">表名称</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colIndex">列号</param>
        /// <param name="content">更新值</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateTableCell(string filePath, string tableName, int rowIndex, int colIndex, string content)
        {
            bool flag = false;
            DataSet ds = new DataSet();
            ds.ReadXml(filePath);
            DataTable dt = ds.Tables[tableName];

            if (dt.Rows[rowIndex][colIndex] != null)
            {
                dt.Rows[rowIndex][colIndex] = content;
                ds.WriteXml(filePath);
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        #endregion
        #endregion

    }

}