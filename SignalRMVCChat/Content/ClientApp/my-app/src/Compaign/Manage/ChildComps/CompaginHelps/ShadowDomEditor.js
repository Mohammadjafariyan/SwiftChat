import root from "react-shadow";
import { _GetSelectedCompaign } from "../../CompaignSave";
import { useState } from "react";
import React, { Component } from "react";
import { Editor } from "primereact/editor";


export default function ShadowDomEditor(props) {
  const [text, settext] = useState(props.text);

  React.useEffect(() => {
    settext(props.text);
  }, [props.text]);

  return (
    <>
      <Editor
        style={{ height: "320px" }}
        value={text}
        onTextChange={(e) => {
          settext(e.htmlValue);

          props.onChange(e.htmlValue);

          _GetSelectedCompaign().Template.Html = e.htmlValue;
        }}
      />
    </>
  );
}
