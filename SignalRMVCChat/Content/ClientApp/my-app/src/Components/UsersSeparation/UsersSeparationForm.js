import React, {Component, useState} from 'react';
import {MyInputG} from "../Utilities/Utility";
import Card from "react-bootstrap/Card";
import {RadioButton} from "primereact/radiobutton";
import {Button, Spinner} from "react-bootstrap";
import {MyCaller, CurrentUserInfo} from "../../Help/Socket";
import {_showError, _showMsg} from "../../Pages/LayoutPage";
import {DataHolder} from "../../Help/DataHolder";
import Container from "react-bootstrap/Container";
import Col from "react-bootstrap/Col";
import Row from "react-bootstrap/Row";

import '../../styles/myStyle.css'
import Table from "react-bootstrap/Table";

class UsersSeparationForm extends Component {
    state = {enabled: true};

    constructor(props) {
        super(props);
        CurrentUserInfo.UsersSeparationForm = this;
    }


    componentDidMount() {

        _showMsg("در حال خواندن اطلاعات")
        this.setState({loading: true})
        MyCaller.Send('GetUsersSeparationForm')

    }

    getUsersSeparationFormCallback(res) {
        this.setState({loading: false})

        if (!res || !res.Content) {
          //  _showError("اطلاعات بازگشتی از سرور دارای خطاست" + "getUsersSeparationForm");
            return;
        }

        this.setState({
            enabled: res.Content.enabled,
            type: res.Content.type,
            RestApiUrl: res.Content.RestApiUrl,
            params: res.Content.params,
            UrlPattern:res.Content.UrlPattern,
            Id:res.Content.Id
        });

    }

    render() {
        return (
            <>

                {this.state.loading &&
                <Spinner animation="border" role="status">
                    <span className="sr-only">لطفا منتظر بمانید ....</span>
                </Spinner>}


                {!this.state.loading &&
                this.renderBody()}

            </>

        );
    }

    renderBody() {
        return <Container>


            <Row>

                <Col>

                </Col>

                <div className={'justify-content-md-center'}>

                    <div className="p-field-radiobutton" style={{display: 'flex'}}>
                        <label>فعال</label>
                        <RadioButton value="فعال" name="enabled" id={'enable'}
                                     onChange={(e) => this.setState({enabled: true})}
                                     checked={this.state.enabled}/>

                        <label>غیر فعال</label> <RadioButton value="غیر فعال" name="enabled"
                                                             onChange={(e) => this.setState({enabled: false})}
                                                             checked={!this.state.enabled}/>
                    </div>
                </div>


                <Col>

                </Col>
            </Row>

            <hr/>

            {
                this.state.enabled &&
                <Row>
                    <Col md={{span: 8, offset: 2, pull: 2}}>
                        <Card >
                            <Card.Header className={'bg-info text-white'}>اطلاعات اصلی</Card.Header>
                            <Card.Body>
                                
                           
                        <MyInputG id={'url-pattern'} title={'الگو تفکیک صفحات '} name={'UrlPattern'} parent={this}
                                  placeholder={'چه صفحاتی مخصوص کاربران عضو است ؟ برای مثال : siteurl.com/dashboard/*'}
                        />
                        <small>در صورت خالی بودن الگوی تفکیک. در همه صفحات فعال خواهد بود </small>


                        <div className={'marginTop'}></div>

                        <div className="p-field-radiobutton">
                            <label>(rest api) استفاده از روش وب سرویس </label>

                            <RadioButton value="(rest api)  استفاده از روش وب سرویس  " name="type" id={'rest-api-type'}
                                         onChange={(e) => this.setState({type: 'rest-api-type'})}
                                         checked={this.state.type === 'rest-api-type'}/>
                        </div>

                        <div className="p-field-radiobutton">
                            <label> css selector استفاده از روش </label>
                            <RadioButton value="  css selector  استفاده از روش   " name="css-selector-type"
                                         onChange={(e) => this.setState({type: 'css-selector-type'})}
                                         checked={this.state.type === 'type'}/>


                        </div>

                        {this.state.type === 'rest-api-type' &&
                        <>
                            <MyInputG id={'rest-api'} title={'آدرس وب سرویس خواندن اطلاعات کاربر'} name={'RestApiUrl'}
                                      parent={this}
                                      placeholder={'برای مثال: siteurl.com/api/getCurrentUserInfo'}
                            />
                        </>}

                            </Card.Body>

                        </Card>


                        <Card >
                            <Card.Header className={'bg-info text-white'}>تعریف پارامتر های قابل خواندن </Card.Header>
                            <Card.Body>

                        <h4>تعریف پارامتر های قابل خواندن </h4>


                        <Button id={'addNewParam'} variant={'light'} onClick={() => {


                            let params = this.state.params;
                            if (!params) {
                                params = [];
                            }

                            params.push({});
                            this.setState({params: params});
                        }}>

                            پارامتر جدید
                            <i className={'fa fa-plus'}></i>
                        </Button>


                        <br/>


                        {this.showParams()}

                            </Card.Body>

                        </Card>

                    </Col>
                </Row>

            }

            <hr/>

            <Button id={'save'} variant={'primary'} onClick={() => {

                this.save();
            }}>
ذخیره
                <i className={'fa fa-check'}></i>
            </Button>

        </Container>
    }


    showParams() {
        if (!this.state.params || !this.state.params.length)
            return;

        return <Table>
            <tbody>
            {this.state.params.filter(p => !p.IsDeleted).map((param, i, arr) => {

                return <tr  >
                   <td>
                       <UserSeparationParameter param={param} onChange={(paramName, paramText, paramType) => {


                           this.state.params[i].paramName = paramName;
                           this.state.params[i].paramText = paramText;
                           this.state.params[i].paramType = paramType;

                           this.setState({params: this.state.params, tn: Math.random()});

                       }}/>
                   </td>

                  <td>
                      <Button variant={'danger'} onClick={() => {

                           let params = this.state.params.filter(p => p !== param)

                        //  this.state.params[i].IsDeleted = true;

                          this.setState({params: params, tn: Math.random()});
                      }}>

                          <i className={'fa fa-minus'}></i>

                      </Button>
                  </td>

                </tr>

            })}
            </tbody>
        </Table>


    }

    save() {

        if (this.state.enabled) {


            if (!this.state.UrlPattern) {
                _showError("خالی بودن الگوی انتخاب ")
            }


            if (!this.state.params || !this.state.params.length) {
                _showError("هیچ پارامتری تعریف نشده است")
                return;
            }

            if (!this.state.type === 'rest-api-type' || !this.state.RestApiUrl) {
                _showError("ادرس وب سرویس وارد نشده است")
                return;
            }

        }


        _showMsg("در حال ثبت")


        this.setState({loading: true});
        MyCaller.Send("SaveUsersSeparationForm", {
            Id:this.state.Id,
            enabled: this.state.enabled,
            type: this.state.type,
            RestApiUrl: this.state.RestApiUrl,
            params: this.state.params,
            UrlPattern:this.state.UrlPattern
        })

    }

    saveUsersSeparationFormCallback(res) {
        this.setState({loading: false});

        _showMsg("ثبت شد")

    }
}

export default UsersSeparationForm;


const UserSeparationParameter = (props) => {

    let fakeParent={state:props.param};

    const [paramType, setParamType] = useState(props.param.paramType);
    const [paramName, setParamName] = useState(props.param.paramName);
    const [paramText, setParamText] = useState(props.param.paramText);

    React.useEffect(() => {
        setParamType(props.param.paramType);
    }, [props.param.paramType])


    React.useEffect(() => {
        setParamName(props.param.paramName);
    }, [props.param.paramName])


    React.useEffect(() => {
        setParamText(props.param.paramText);
    }, [props.param.paramText])


    return <>


        <div className="p-field-radiobutton" style={{display: 'flex'}}>

            <label>پارامتر وب سرویس</label>
            <RadioButton value="پارامتر وب سرویس" name="param-type"
                         className={'param-type-rest'}
                         onChange={(e) => {
                             setParamType('param-type-rest')
                             props.onChange(paramName, paramText, 'param-type-rest');

                         }}
                         checked={paramType === 'param-type-rest'}/>

            <label>پارامتر css selector</label>
            <RadioButton value="پارامتر css selector" name="param-type"
                         className={'param-type-css'}
                         onChange={(e) => {
                             setParamType('param-type-css')
                             props.onChange(paramName, paramText, 'param-type-css');

                         }}
                         checked={paramType === 'param-type-css'}/>


        </div>
        {paramType === 'param-type-css' &&
        <MyInputG id={'param-name'} className={'param-name'} title={'نام پارامتر در  css selector'} name={'paramName'}
                  parent={fakeParent}
                  placeholder={'ایی که خوانده خواهد شد(Unique) نام یکتای  '} onChange={(val) => {

            setParamName(val);


            props.onChange(val, paramText, paramType);
        }}
        />}


        {paramType === 'param-type-rest' &&
        <MyInputG id={'param-name'} className={'param-name'} title={'نام پارامتر در وب سرویس'} name={'paramName'}
                  parent={fakeParent}
                  placeholder={'ایی که خوانده خواهد شد(Unique) نام یکتای  '} onChange={(val) => {

            setParamName(val);
            props.onChange(val, paramText, paramType);
        }}
        />}


        <MyInputG id={'param-text'}  className={'param-text'} title={'نام فارسی پارامتر'} name={'paramText'} parent={fakeParent}
                  placeholder={'ایی که در قسمت داشبورد نمایش داده خواهد شد(Unique) نام یکتای  '}
                  onChange={(val) => {

                      setParamText(val);
                      props.onChange(paramName, val, paramType);
                  }}
        />


    </>


}