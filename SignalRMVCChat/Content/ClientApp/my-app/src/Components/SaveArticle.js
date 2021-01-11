import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import {Input} from "./FormInputs";
import {Editor} from "primereact/editor";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import {Button} from "react-bootstrap";
import {_showError, _showMsg} from "../Pages/LayoutPage";
import Spinner from "react-bootstrap/Spinner";
import {MyCaller} from "../Help/Socket";
import Col from "react-bootstrap/Col";
import {_getHelpDesk} from "../Pages/HelpDeskPage";
import {Dropdown} from "primereact/dropdown";
import {CurrentUserInfo} from "../CurrentUserInfo";

class SaveArticle extends Component {

    state = {
        
        /*Publish=2,Hidden=1,Draft=0*/
        ArticleStatusList:[
            {Title:'انتشار',Value:2},
            {Title:'مخفی',Value:1},
            {Title:'پیش نویس',Value:0},
        ]
    }

    constructor(ar) {
        super(ar);
        CurrentUserInfo.SaveArticle = this;
    }

    articleGetByIdCallback(res) {
        if (!res || !res.Content) {
            _showError("ers is null hD_GetByIdCallback ");
            return;
        }
       


        this.setState({wait: false});

        DataHolder.HelpDeskRowDataSelected = res.Content;

        this.preSelectHelpDesk();
        
        
       let ArticleStatus= this.state.ArticleStatusList.find(f=>f.Value===DataHolder.HelpDeskRowDataSelected.ArticleStatus);

        this.setState({tmp: Math.random(),ArticleStatus:ArticleStatus});


    }

    preSelectHelpDesk() {

        if (!this.state.categories || !DataHolder.HelpDeskRowDataSelected ) {
            return;
        }

        var category = this.state.categories.find(c => c.Id === DataHolder.HelpDeskRowDataSelected.CategoryId)
        
        if (category) {
            this.setState({selectedCategory: category});
        }
    }

    componentDidMount() {

        if (DataHolder.HelpDeskRowDataSelected && DataHolder.HelpDeskRowDataSelected.Id) {
            this.setState({wait: true});

            _showMsg("در حال خواندن مقاله")
            //todo:
            MyCaller.Send('ArticleGetById', {id: DataHolder.HelpDeskRowDataSelected.Id})
        }else{
            DataHolder.HelpDeskRowDataSelected={};
        }


        this.loadCategories();

    }


    save() {
        if (!this.state.selectedCategory) {
            _showError('لطفا دسته بندی را انتخاب کنید');
            return;
        }

        if (isBlank(DataHolder.HelpDeskRowDataSelected.Title)) {
            _showError('عنوان خالی است');
            return;
        }
        if (!_getHelpDesk().helpDeskId) {
            _showError('هیچ مرکز پشتیبانی ای انتخاب نشده است');
            return;
        }


        DataHolder.HelpDeskRowDataSelected.HelpDeskId = _getHelpDesk().helpDeskId;

        DataHolder.HelpDeskRowDataSelected.CategoryId= this.state.selectedCategory.Id;
        
        this.setState({wait: true});

        _showMsg("در حال ذخیره مقاله")

        
        //todo:
        MyCaller.Send('ArticleSave',
            DataHolder.HelpDeskRowDataSelected)

    }


    loadCategories() {
        if (!_getHelpDesk().helpDeskId) {
            _showError("هیچ مرکز پشتیبانی انتخاب نشده است ، سمت کلاینت")
            return;
        }

        //todo:
        MyCaller.Send("Category_Get_List", {helpDeskId: _getHelpDesk().helpDeskId})

    }

    category_Get_List_Callback(res) {
        this.setState({loading: false})

        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }


        this.setState({categories: res.Content})


        setTimeout(()=>{
            this.preSelectHelpDesk();
        },1000)

    }

    //todo:
    articleSaveCallback(res) {

        this.setState({wait: false});

        _showMsg("با موفقیت ذخیره شد")

        this.back();

        DataHolder.HelpDeskRowDataSelected={};

    }

    showTitle() {
        if (DataHolder.HelpDeskRowDataSelected &&
            DataHolder.HelpDeskRowDataSelected.Id)
            return <h4>فرم ویرایش مقاله</h4>

        return <h4>فرم ثبت مقاله</h4>
    }

    back() {
        DataHolder.subPage = null;

        //   DataHolder.HelpDeskRowDataSelected = rowData;

        CurrentUserInfo.LayoutPage.setState({tmp: Math.random()});
    }

    render() {

        if (!DataHolder.HelpDeskRowDataSelected) {
            /*  return  <p>هیچ مقاله ای انتخاب نشده است</p>*/
            DataHolder.HelpDeskRowDataSelected = {};
        }

        return (
            <div dir={'rtl'}>


                <Container>


                    {this.showTitle()}


                    <small>بعد از اعمال تغییرات دکمه ثبت را فشار دهید</small>


                    <button style={{float: 'left'}} type={'button'} className="btn btn-light"

                            onClick={() => {

                                this.back();
                            }}>
                        بازگشت
                        <i className="fa fa-arrow-left" aria-hidden="true"></i>
                    </button>


                    {this.state.wait &&

                    <Spinner animation="border" role="status">
                        <span className="sr-only">لطفا منتظر بمانید...</span>
                    </Spinner>
                    }
                    <br/>
                    <br/>
                    <hr/>
                    <Row>
                        <Col>
                            <Input 
                            id="articleTitle"
                            value={DataHolder.HelpDeskRowDataSelected.Title}
                                   type={'text'} name={'عنوان مقاله'}

                                   onChange={(e) => {
                                       DataHolder.HelpDeskRowDataSelected.Title = e.target.value;
                                       this.setState({tmp: Math.random()});
                                   }}

                                   placeholder={''}></Input>
                        </Col>


                        <Col>
                            <label>انتخاب دسته بندی</label>
                            <br/>
                            <Dropdown value={this.state.selectedCategory}
                                      options={this.state.categories}
                                      onChange={(e) => {

                                          this.setState({selectedCategory: e.value});

                                      }} optionLabel="Title" placeholder="انتخاب دسته بندی"/>


                        </Col>
                        <Col>
                            <label>انتخاب دسته بندی</label>
                            <br/>
                            <Dropdown value={this.state.ArticleStatus}
                                      options={this.state.ArticleStatusList}
                                      onChange={(e) => {

                                          this.setState({ArticleStatus: e.value});
                                          DataHolder.HelpDeskRowDataSelected.ArticleStatus= e.value.Value

                                      }} optionLabel="Title" placeholder="نحوه انتشار"/>


                        </Col>
                    </Row>
                    
                    <br/>
                    <Row>
                        <Button id="saveArticleOk" variant={'primary'} onClick={() => {

                            this.save();
                        }}>
                            ثبت
                        </Button>
                    </Row>
                    <hr/>
                    <Editor style={{height: '320px'}}
                            value={DataHolder.HelpDeskRowDataSelected.Content}
                            onTextChange={(e) =>

                            {
                                DataHolder.HelpDeskRowDataSelected.textValue=e.textValue;
                                DataHolder.HelpDeskRowDataSelected.Content=e.htmlValue;

                                this.setState({Content: e.htmlValue});
                            }
                                }/>


                </Container>

            </div>
        );
    }
}

export default SaveArticle;

export function isBlank(str) {
    return (!str || /^\s*$/.test(str));
}
