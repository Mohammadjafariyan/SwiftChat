import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import {Input} from "./FormInputs";
import {Editor} from "primereact/editor";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import {Button} from "react-bootstrap";
import {_showError, _showMsg} from "../Pages/LayoutPage";
import Spinner from "react-bootstrap/Spinner";
import {CurrentUserInfo, MyCaller} from "../Help/Socket";

class SaveArticle extends Component {
   
    state={}
    
    componentDidMount() {
        
        this.setState({wait:true});
        
        _showMsg("در حال خواندن مقاله")
        //todo:
        MyCaller.Send('HD_GetById',{id:DataHolder.HelpDeskRowDataSelected.Id})
    }

//todo:
    hD_GetByIdCallback(res){
        if (!res || !res.Content){
            _showError("ers is null hD_GetByIdCallback ");
            return ;
        }

        this.setState({wait:false});

        DataHolder.HelpDeskRowDataSelected=res.Content;

        this.setState({tmp: Math.random()});
        
    }

    save(){
        this.setState({wait:true});

        _showMsg("در حال ذخیره مقاله")
      
        //todo:
        MyCaller.Send('HD_Save',
            {HelpDeskRowDataSelected:DataHolder.HelpDeskRowDataSelected})

    }

    //todo:
    hD_SaveCallback(res){
        
        this.setState({wait:false});

        _showMsg("با موفقیت ذخیره شد")
    }
    

    render() {
        
        if (!DataHolder.HelpDeskRowDataSelected){
            return  <p>هیچ مقاله ای انتخاب نشده است</p>
        }
        
        return (
            <div dir={'rtl'}>

                
                <Container>
                    <h4>فرم ثبت و ویرایش مقاله</h4>
                    
                    <small>بعد از اعمال تغییرات دکمه ثبت را فشار دهید</small>
                    
                    
                    {this.state.wait &&

                    <Spinner animation="border" role="status">
                        <span className="sr-only">لطفا منتظر بمانید...</span>
                    </Spinner>
                    }
                    <Row>
                            <Input value={DataHolder.HelpDeskRowDataSelected.title}
                                   type={'text'} name={'عنوان مقاله'}

                                   onChange={(e)=>{
                                       DataHolder.HelpDeskRowDataSelected.title=e.target.value;
                                       this.setState({tmp:Math.random()});
                                   }}

                                   placeholder={''}></Input>
                    </Row>

                    <br/>
                    <Row>
                        <Button variant={'primary'} onClick={()=>{
                            
                            this.save();
                        }}>
                            ثبت
                        </Button>
                    </Row>
                            <hr/>
                    <Editor  style={{ height: '320px' }}
                                     value={this.state.Article}
                                     onTextChange={(e) => 
                                         
                                         this.setState({ Article: e.htmlValue })} />


                </Container>
                
            </div>
        );
    }
}

export default SaveArticle;