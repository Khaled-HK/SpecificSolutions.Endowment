import{u as g,S as v,P as c,U as k,E as b}from"./index-CPwL1tHs.js";import{T as C}from"./index-CdVeab-9.js";import{V as n}from"./VChip-f5OQAicY.js";import{V as y}from"./VDivider-dlsVqP_0.js";import{c as u,o as d,l as $,b as i,m as e,e as r,t as s,r as A,aR as x,f as E}from"./index-BxwAfQkT.js";import{_ as B}from"./AppCardCode-DtgCP9Zg.js";import{_ as V}from"./TiptapEditor-B-ZFoKgd.js";import{V as w,a as m}from"./VRow-IwBDz7dS.js";import"./VSlideGroup-DvcRT41v.js";import"./VAvatar-D3B_nxVk.js";import"./VImg-BT79GK2N.js";import"./vue3-perfect-scrollbar-DA0_kQac.js";import"./VCard-HyXZyDus.js";import"./VCardText-Br0ga3MS.js";/* empty css              */const H={class:"border pa-2 rounded custom-editor"},I={key:0,class:"d-flex flex-wrap gap-x-4 gap-y-2 mb-2"},T={__name:"DemoEditorCustomEditor",setup(p){const t=g({content:`
        <h2>
          Hi there,
        </h2>
        <p>
          this is a <em>basic</em> example of <strong>tiptap</strong>. Sure, there are all kind of basic text styles you'd probably expect from a text editor. But wait until you see the lists:
        </p>
        <ul>
          <li>
            That's a bullet list with one …
          </li>
          <li>
            … or two list items.
          </li>
        </ul>
        <p>
          Isn't that great? And all of that is editable. But wait, there's more. Let's try a code block:
        </p>
        <pre><code class="language-css">body {
  display: none;
}</code></pre>
        <p>
          I know, I know, this is impressive. It's only the tip of the iceberg though. Give it a try and click a little bit around. Don't forget to check the other examples too.
        </p>
        <blockquote>
          Wow, that's amazing. Good work, boy! 👏
          <br />
          — Mom
        </blockquote>
      `,extensions:[v,C.configure({types:["heading","paragraph"]}),c.configure({placeholder:"Write something here..."}),k]});return(f,o)=>(d(),u("div",H,[e(t)?(d(),u("div",I,[i(n,{disabled:!e(t).can().chain().focus().toggleBold().run(),color:e(t).isActive("bold")?"primary":"",onClick:o[0]||(o[0]=l=>e(t).chain().focus().toggleBold().run())},{default:r(()=>o[21]||(o[21]=[s(" bold ")])),_:1},8,["disabled","color"]),i(n,{disabled:!e(t).can().chain().focus().toggleItalic().run(),color:e(t).isActive("italic")?"primary":"",onClick:o[1]||(o[1]=l=>e(t).chain().focus().toggleItalic().run())},{default:r(()=>o[22]||(o[22]=[s(" italic ")])),_:1},8,["disabled","color"]),i(n,{disabled:!e(t).can().chain().focus().toggleStrike().run(),color:e(t).isActive("strike")?"primary":"",onClick:o[2]||(o[2]=l=>e(t).chain().focus().toggleStrike().run())},{default:r(()=>o[23]||(o[23]=[s(" strike ")])),_:1},8,["disabled","color"]),i(n,{disabled:!e(t).can().chain().focus().toggleCode().run(),color:e(t).isActive("code")?"primary":"",onClick:o[3]||(o[3]=l=>e(t).chain().focus().toggleCode().run())},{default:r(()=>o[24]||(o[24]=[s(" code ")])),_:1},8,["disabled","color"]),i(n,{onClick:o[4]||(o[4]=l=>e(t).chain().focus().unsetAllMarks().run())},{default:r(()=>o[25]||(o[25]=[s(" clear marks ")])),_:1}),i(n,{onClick:o[5]||(o[5]=l=>e(t).chain().focus().clearNodes().run())},{default:r(()=>o[26]||(o[26]=[s(" clear nodes ")])),_:1}),i(n,{color:e(t).isActive("paragraph")?"primary":"",onClick:o[6]||(o[6]=l=>e(t).chain().focus().setParagraph().run())},{default:r(()=>o[27]||(o[27]=[s(" paragraph ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("heading",{level:1})?"primary":"",onClick:o[7]||(o[7]=l=>e(t).chain().focus().toggleHeading({level:1}).run())},{default:r(()=>o[28]||(o[28]=[s(" h1 ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("heading",{level:2})?"primary":"",onClick:o[8]||(o[8]=l=>e(t).chain().focus().toggleHeading({level:2}).run())},{default:r(()=>o[29]||(o[29]=[s(" h2 ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("heading",{level:3})?"primary":"",onClick:o[9]||(o[9]=l=>e(t).chain().focus().toggleHeading({level:3}).run())},{default:r(()=>o[30]||(o[30]=[s(" h3 ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("heading",{level:4})?"primary":"",onClick:o[10]||(o[10]=l=>e(t).chain().focus().toggleHeading({level:4}).run())},{default:r(()=>o[31]||(o[31]=[s(" h4 ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("heading",{level:5})?"primary":"",onClick:o[11]||(o[11]=l=>e(t).chain().focus().toggleHeading({level:5}).run())},{default:r(()=>o[32]||(o[32]=[s(" h5 ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("heading",{level:6})?"primary":"",onClick:o[12]||(o[12]=l=>e(t).chain().focus().toggleHeading({level:6}).run())},{default:r(()=>o[33]||(o[33]=[s(" h6 ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("bulletList")?"primary":"",onClick:o[13]||(o[13]=l=>e(t).chain().focus().toggleBulletList().run())},{default:r(()=>o[34]||(o[34]=[s(" bullet list ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("orderedList")?"primary":"",onClick:o[14]||(o[14]=l=>e(t).chain().focus().toggleOrderedList().run())},{default:r(()=>o[35]||(o[35]=[s(" ordered list ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("codeBlock")?"primary":"",onClick:o[15]||(o[15]=l=>e(t).chain().focus().toggleCodeBlock().run())},{default:r(()=>o[36]||(o[36]=[s(" code block ")])),_:1},8,["color"]),i(n,{color:e(t).isActive("blockquote")?"primary":"",onClick:o[16]||(o[16]=l=>e(t).chain().focus().toggleBlockquote().run())},{default:r(()=>o[37]||(o[37]=[s(" blockquote ")])),_:1},8,["color"]),i(n,{onClick:o[17]||(o[17]=l=>e(t).chain().focus().setHorizontalRule().run())},{default:r(()=>o[38]||(o[38]=[s(" horizontal rule ")])),_:1}),i(n,{onClick:o[18]||(o[18]=l=>e(t).chain().focus().setHardBreak().run())},{default:r(()=>o[39]||(o[39]=[s(" hard break ")])),_:1}),i(n,{disabled:!e(t).can().chain().focus().undo().run(),onClick:o[19]||(o[19]=l=>e(t).chain().focus().undo().run())},{default:r(()=>o[40]||(o[40]=[s(" undo ")])),_:1},8,["disabled"]),i(n,{disabled:!e(t).can().chain().focus().redo().run(),onClick:o[20]||(o[20]=l=>e(t).chain().focus().redo().run())},{default:r(()=>o[41]||(o[41]=[s(" redo ")])),_:1},8,["disabled"])])):$("",!0),i(y,{class:"my-4"}),i(e(b),{editor:e(t)},null,8,["editor"])]))}},q={__name:"DemoEditorBasicEditor",setup(p){const t=A(`
<p>
  This is a radically reduced version of tiptap. It has support for a document, with paragraphs and text. That's it. It's probably too much for real minimalists though.
</p>
<p>
  The paragraph extension is not really required, but you need at least one node. Sure, that node can be something different.  
</p>
`);return(f,o)=>{const l=V;return d(),u("div",null,[i(l,{modelValue:e(t),"onUpdate:modelValue":o[0]||(o[0]=a=>x(t)?t.value=a:null),class:"border rounded basic-editor"},null,8,["modelValue"])])}}},D={},S={},Q={__name:"editors",setup(p){return(t,f)=>{const o=q,l=B,a=T;return d(),E(w,null,{default:r(()=>[i(m,{cols:"12"},{default:r(()=>[i(l,{title:"Basic Editor",code:D},{default:r(()=>[i(o)]),_:1},8,["code"])]),_:1}),i(m,{cols:"12"},{default:r(()=>[i(l,{title:"Custom Editor",code:S},{default:r(()=>[i(a)]),_:1},8,["code"])]),_:1})]),_:1})}}};export{Q as default};
