#介绍

此处用于资源汉化所需的文件。

文件数量可以根据需要任意建立，通常建议同类的cfg创建一个文件。


###Parts.xml

组件内容的汉化

汉化的文件：Squad\Parts

###ScienceDefs.xml

科技探索内容汉化

汉化的文件：Squad\Resources\ScienceDefs.cfg

###TechTree.xml  

尚不明确此处条目用于游戏那里

汉化的文件：Squad\Resources\TechTree.cfg


#xml结构介绍

    <Configs>
        <!--通常一个文件对应一个Config。-->
        <!--属性url和name任选一个即可，默认url优先。url为组件的绝对地址，不会遇到重名问题，那么是组件的名称，理论上有遇到重名问题，遇到重名程序只会处理第一个。-->
        <!--属性id可选，当多条数据使用相同的url是，可以使用这个id去匹配这条数据下的id参数来定位资源条目-->
        <Config name="DemoConfigName" url="Squad/Resources/TechTree/TechTree">
            <!--属性集合的汉化-->
            <Values>
                <!--一条数据汉化一个值，属性name必填-->
                <Value name="title"><![CDATA[舱内报告]]></Value>
            </Values>
            <!--子节点使用Nodes进行嵌套，它可以无限极嵌套，根据资源实际层次结构决定-->
            <Nodes>
                <!--节点定位，name和id二选一，优先id【类似Config定位方式】-->
                <Node name="DemoNodeName" id="start">
                    <Values>
                        <Value name="title"><![CDATA[开始]]></Value>
                        <Value name="description"><![CDATA[技术开始。]]></Value>
                    </Values>
                </Node>
            </Nodes>
        </Config>
    </Configs>
