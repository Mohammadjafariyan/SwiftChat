import React, {Component, useState} from 'react';
import {Row, Col, Container, Form, Button} from 'react-bootstrap';
import {MyCaller} from './../Help/Socket';
import {CurrentUserInfo} from "../CurrentUserInfo";

const LoadImageAndSet = function (e2, callback) {
    let elementById = e2;

    if (elementById.target.files.length > 1) {

        CurrentUserInfo.LayoutPage.showError('فقط یک عکس مجاز است');
        return;
    }

    for (let i = 0; i < elementById.target.files.length; i++) {
        let file = elementById.target.files[i];


        var fileReader = new FileReader();
        fileReader.onload = function (e) {
            console.log(e.target.result)

            callback(e.target.result);
        }

        fileReader.readAsDataURL(file);


    }
}

class ProfilePage extends Component {


    constructor(props, context) {
        super(props, context);
        CurrentUserInfo.ProfilePage=this;
        this.state={};
    }

    componentDidMount() {
        CurrentUserInfo.LayoutPage.showMsg('در حال خواندن اطلاعات پروفایل');
        MyCaller.Send('GetMyProfile')
    }

    getMyProfileCallback(res){
        if (!res || !res.Content){
            CurrentUserInfo.LayoutPage.showError('res is null for profile');
            return ;
        }
        CurrentUserInfo.LayoutPage.showMsg('اطلاعات پروفایل خوانده شد');

        this.setState({Name:res.Content.Name,Image:res.Content.Image,
        MyAccount:res.Content});
    }

    render() {
        return (
            <Container fluid>
                <Row>
                    <Col>
                        {this.state.Image && 
                        <img src={this.state.Image} width='300' height='200'  />}
                        
                    </Col>
                    <Col>
                        {this.state.Name && 
                        <ProfilePageForm ImageLoad={(content)=>{
                            this.setState({Image:content})
                            
                        }} Name={this.state.Name} Img={this.state.Image}  
                                         onSubmit={(name,image,Img)=>this.onMSubmit(name,image,Img)}></ProfilePageForm> }
                    </Col>

                    <Col></Col>
                </Row>


            </Container>
        );
    }

    onMSubmit(name, image, Img) {
        if (!name){
            CurrentUserInfo.LayoutPage.showError('نام وارد نشده است');
            return ;
        }
        CurrentUserInfo.LayoutPage.showMsg('در حال ذخیره تغییرات پروفایل');
        MyCaller.Send('SaveMyProfile',{name,image,Img});

    }

    saveMyProfileCallback(res){
        this.getMyProfileCallback(res);
        CurrentUserInfo.LayoutPage.showMsg('تغییرات پروفایل با موفقیت ذخیره شد');
    }
}

function ProfilePageForm(props) {
    const [name, setName] = useState(props.Name);
    const [image, setImage] = useState('');

    const Img=props.Img;
    
    
    return (

        <Form>
            <Form.Group controlId="formBasicEmail">
                <Form.Label>نام شما</Form.Label>
                <Form.Control value={name} type="text" placeholder="نام شما" onChange={(e) => setName(e.target.value)}/>
                <Form.Text className="text-muted">به بازدید کنندگان نمایش داده می شود</Form.Text>
            </Form.Group>

            <Form.Group>
                <Form.File  label="عکس پروفایل" onChange={(e) => {

                    LoadImageAndSet(e, (fileContent) => {
                            setImage(fileContent);

                        props.ImageLoad(fileContent)
                        }
                    );
                }} accept='.jpeg,.jpg,.png'/>
            </Form.Group>
            <Button variant="primary" type="button" onClick={()=>{
                props.onSubmit(name,image,Img);
            }} >
                ذخیره
            </Button>
        </Form>

    );
}

export default ProfilePage;
