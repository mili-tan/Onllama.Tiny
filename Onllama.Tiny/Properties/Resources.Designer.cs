﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Onllama.Tiny.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Onllama.Tiny.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;{{ .System }}
        ///User: {{ .Prompt }}
        ///
        ///Assistant:
        ///&quot;&quot;&quot;
        ///PARAMETER num_ctx 4096 的本地化字符串。
        /// </summary>
        internal static string deekseekTmp {
            get {
                return ResourceManager.GetString("deekseekTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;{{ if .System }}{{ .System }}
        ///
        ///{{ end }}{{ if .Prompt }}User: {{ .Prompt }}
        ///
        ///{{ end }}Assistant: {{ .Response }}&quot;
        ///PARAMETER stop User:
        ///PARAMETER stop Assistant:
        ///PARAMETER num_gpu 0 的本地化字符串。
        /// </summary>
        internal static string deekseekv2Tmp {
            get {
                return ResourceManager.GetString("deekseekv2Tmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;&lt;start_of_turn&gt;user
        ///{{ if .System }}{{ .System }} {{ end }}{{ .Prompt }}&lt;end_of_turn&gt;
        ///&lt;start_of_turn&gt;model
        ///{{ .Response }}&lt;end_of_turn&gt;
        ///&quot;&quot;&quot;
        ///PARAMETER stop &quot;&lt;start_of_turn&gt;&quot;
        ///PARAMETER stop &quot;&lt;end_of_turn&gt;&quot;
        ///PARAMETER repeat_penalty 1 的本地化字符串。
        /// </summary>
        internal static string gemmaTmp {
            get {
                return ResourceManager.GetString("gemmaTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;[INST] {{ .System }} {{ .Prompt }} [/INST]&quot;&quot;&quot;
        ///PARAMETER stop &quot;[INST]&quot;
        ///PARAMETER stop &quot;[/INST]&quot; 的本地化字符串。
        /// </summary>
        internal static string mistralTmp {
            get {
                return ResourceManager.GetString("mistralTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;{{ if .System }}&lt;|im_start|&gt;system
        ///{{ .System }}&lt;|im_end|&gt;
        ///{{ end }}{{ if .Prompt }}&lt;|im_start|&gt;user
        ///{{ .Prompt }}&lt;|im_end|&gt;
        ///{{ end }}&lt;|im_start|&gt;assistant
        ///{{ .Response }}&lt;|im_end|&gt;
        ///&quot;&quot;&quot;
        ///PARAMETER num_gpu 0
        ///PARAMETER stop &quot;&lt;|im_start|&gt;&quot;
        ///PARAMETER stop &quot;&lt;|im_end|&gt;&quot; 的本地化字符串。
        /// </summary>
        internal static string qwen2Tmp {
            get {
                return ResourceManager.GetString("qwen2Tmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;{{ if .System }}&lt;|im_start|&gt;system
        ///{{ .System }}&lt;|im_end|&gt;{{ end }}&lt;|im_start|&gt;user
        ///{{ .Prompt }}&lt;|im_end|&gt;
        ///&lt;|im_start|&gt;assistant
        ///&quot;&quot;&quot;
        ///PARAMETER stop &quot;&lt;|im_start|&gt;&quot;
        ///PARAMETER stop &quot;&lt;|im_end|&gt;&quot; 的本地化字符串。
        /// </summary>
        internal static string qwenTmp {
            get {
                return ResourceManager.GetString("qwenTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;?xml version=&quot;1.0&quot; standalone=&quot;no&quot;?&gt;&lt;!DOCTYPE svg PUBLIC &quot;-//W3C//DTD SVG 1.1//EN&quot; &quot;http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd&quot;&gt;&lt;svg t=&quot;1710874848376&quot; class=&quot;icon&quot; viewBox=&quot;0 0 1024 1024&quot; version=&quot;1.1&quot; xmlns=&quot;http://www.w3.org/2000/svg&quot; p-id=&quot;8348&quot; xmlns:xlink=&quot;http://www.w3.org/1999/xlink&quot; width=&quot;200&quot; height=&quot;200&quot;&gt;&lt;path d=&quot;M924.8 625.7l-65.5-56c3.1-19 4.7-38.4 4.7-57.8s-1.6-38.8-4.7-57.8l65.5-56c10.1-8.6 13.8-22.6 9.3-35.2l-0.9-2.6c-18.1-50.5-44.9-96.9-79.7-137.9l-1.8-2.1c-8.6-10.1-22.5-13.9-35.1-9. [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string settingSvg {
            get {
                return ResourceManager.GetString("settingSvg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;{{ if .System }}&lt;|im_start|&gt;system
        ///{{ .System }}&lt;|im_end|&gt;
        ///{{ end }}{{ if .Prompt }}&lt;|im_start|&gt;user
        ///{{ .Prompt }}&lt;|im_end|&gt;
        ///{{ end }}&lt;|im_start|&gt;assistant
        ///{{ .Response }}&lt;|im_end|&gt;
        ///&quot;&quot;&quot;
        ///PARAMETER stop &quot;&lt;|im_start|&gt;&quot;
        ///PARAMETER stop &quot;&lt;|im_end|&gt;&quot; 的本地化字符串。
        /// </summary>
        internal static string yi15Tmp {
            get {
                return ResourceManager.GetString("yi15Tmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;&lt;|im_start|&gt;system
        ///{{ .System }}&lt;|im_end|&gt;
        ///&lt;|im_start|&gt;user
        ///{{ .Prompt }}&lt;|im_end|&gt;
        ///&lt;|im_start|&gt;assistant
        ///&quot;&quot;&quot;
        ///PARAMETER stop &quot;&lt;|im_start|&gt;&quot;
        ///PARAMETER stop &quot;&lt;|im_end|&gt;&quot; 的本地化字符串。
        /// </summary>
        internal static string yiTmp {
            get {
                return ResourceManager.GetString("yiTmp", resourceCulture);
            }
        }
    }
}
