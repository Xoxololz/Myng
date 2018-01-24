<?xml version="1.0" encoding="UTF-8"?>
<tileset name="terrain" tilewidth="32" tileheight="32" tilecount="1024" columns="32">
 <image source="../tilesets/terrain.png" width="1024" height="1024"/>
 <terraintypes>
  <terrain name="cesta" tile="0"/>
  <terrain name="voda" tile="1441"/>
  <terrain name="tráva" tile="1441"/>
 </terraintypes>
 <tile id="7" terrain="0,0,0,"/>
 <tile id="8" terrain="0,0,,0"/>
 <tile id="28" terrain="1,1,1,">
  <objectgroup draworder="index">
   <object id="1" x="0.25" y="-0.25">
    <polygon points="0,0 31.75,0.5 32,20.75 23.75,33 -0.5,32.25"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="29" terrain="1,1,,1">
  <objectgroup draworder="index">
   <object id="1" x="-0.25" y="-0.25">
    <polygon points="0,0 32.5,0.25 32.5,32.5 9.75,32.5 0.75,19.25"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="39" terrain="0,,0,0"/>
 <tile id="40" terrain=",0,0,0"/>
 <tile id="60" terrain="1,,1,1">
  <objectgroup draworder="index">
   <object id="1" x="18.25" y="-1">
    <polygon points="0,0 14,12.75 13.75,33 -18,33.25 -18.25,1.75"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="61" terrain=",1,1,1">
  <objectgroup draworder="index">
   <object id="1" x="9.25" y="0">
    <polygon points="0,0 -8.5,9.25 -9,31.5 22.75,32 23,0.25"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="70" terrain=",,,0"/>
 <tile id="71" terrain=",,0,0"/>
 <tile id="72" terrain=",,0,"/>
 <tile id="91" terrain=",,,1">
  <objectgroup draworder="index">
   <object id="1" x="31.5" y="7.75">
    <polygon points="0,0 -23.25,24 1.5,24.5"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="92" terrain=",,1,1">
  <objectgroup draworder="index">
   <object id="1" x="0" y="8.75">
    <polygon points="0,0 32.75,-0.75 32.25,23 0,22.75"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="93" terrain=",,1,">
  <objectgroup draworder="index">
   <object id="1" x="0.25" y="7.75">
    <polygon points="0,0 23.75,24.5 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="102" terrain=",0,,0"/>
 <tile id="103" terrain="0,0,0,0"/>
 <tile id="104" terrain="0,,0,"/>
 <tile id="123" terrain=",1,,1">
  <objectgroup draworder="index">
   <object id="1" x="9.25" y="-0.25">
    <polygon points="0,0 2,31.75 23.25,32.25 23,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="124" terrain="1,1,1,1">
  <objectgroup draworder="index">
   <object id="1" x="0" y="-0.25">
    <polygon points="0,0 31.75,0 32.5,32 0.25,32.5"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="125" terrain="1,,1,">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0.25">
    <polygon points="0,0 21,-0.5 23.75,32 0,31.75"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="134" terrain=",0,,"/>
 <tile id="135" terrain="0,0,,"/>
 <tile id="136" terrain="0,,,"/>
 <tile id="155" terrain=",1,,">
  <objectgroup draworder="index">
   <object id="1" x="10.25" y="-0.25">
    <polygon points="0,0 21.75,21.75 22.25,0.75"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="156" terrain="1,1,,">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0.5" width="32.5" height="19.5"/>
  </objectgroup>
 </tile>
 <tile id="157" terrain="1,,,">
  <objectgroup draworder="index">
   <object id="1" x="0" y="-0.25">
    <polygon points="0,0 0,21.75 25,0.25"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="205" terrain="2,2,2,"/>
 <tile id="206" terrain="2,2,,2"/>
 <tile id="237" terrain="2,,2,2"/>
 <tile id="238" terrain=",2,2,2"/>
 <tile id="268" terrain=",,,2"/>
 <tile id="269" terrain=",,2,2"/>
 <tile id="270" terrain=",,2,"/>
 <tile id="300" terrain=",2,,2"/>
 <tile id="301" terrain="2,2,2,2"/>
 <tile id="302" terrain="2,,2,"/>
 <tile id="332" terrain=",2,,"/>
 <tile id="333" terrain="2,2,,"/>
 <tile id="334" terrain="2,,,"/>
</tileset>
