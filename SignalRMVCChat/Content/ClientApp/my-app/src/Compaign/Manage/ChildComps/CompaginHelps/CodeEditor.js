import React, { Component } from 'react'

import Editor from 'react-simple-code-editor';
import { highlight, languages } from 'prismjs/components/prism-core';
import {Card} from 'primereact/card';

import 'prismjs/components/prism-clike';


import 'prismjs/components/prism-javascript';
import 'prismjs/components/prism-markup';
import 'prismjs/components/prism-css';
import 'prismjs/themes/prism.css';

require('prismjs/components/prism-jsx');


 
const code = `function add(a, b) {
  return a + b;
}
`;
export default class CodeEditor extends Component {
    state = { code };


    componentDidMount(){

        this.setState({
            code:this.props.code ? this.props.code : `
            
            <p><span style="color: rgb(0, 102, 204);">متن نمونه</span></p>
           ` 
        })

    }

    componentWillUnmount(){
        this.props.onChange(this.state.code);

    }

    render() {
        return (
            <Card style={{maxHeight:'350px' , overflow:'auto'}}>
                   <Editor
        value={this.state.code}
        onValueChange={code => {
            this.setState({ code });

            this.props.onChange(code);
        }}
        highlight={code =>{
          code=  highlight(code, languages.jsx);

          return code;
        }}
        padding={10}
        style={{
          fontFamily: '"Fira code", "Fira Mono", monospace',
          fontSize: 12,
        }}
      />
            </Card>
        )
    }
}
