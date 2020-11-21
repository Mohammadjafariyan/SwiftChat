import React, {Component} from 'react';
import Dropdown from "react-bootstrap/Dropdown";
import Row from "react-bootstrap/cjs/Row";
import Col from "react-bootstrap/Col";
import {InputMask} from "primereact/inputmask";
import Button from "react-bootstrap/cjs/Button";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";

class AddUrlRoute extends Component {
    constructor(props) {
        super(props);
        this.state = {
            type: null,
        };

        this.types = [
            {name: 'شامل', code: 'contains'},
            {name: 'برابر با', code: 'equals'},
        ];

        this.onCityChange = this.onCityChange.bind(this);
        this.typeTemplate = this.typeTemplate.bind(this);
        this.actionTemplate = this.actionTemplate.bind(this);


    }

    typeTemplate(row) {
        if (!row.type) {
            return <></>
        }
        return <span>{row.type.name}</span>
    }

    onCityChange(e) {
        this.setState({type: e.value});
    }

    componentDidMount() {
        if (!this.props.parent.selected.UrlRoutes) {
            this.props.parent.selected.UrlRoutes = [];
        }
    }

    actionTemplate(row) {

        return <Button onClick={() => {

            this.props.parent.selected.UrlRoutes = this.props.parent.selected.UrlRoutes.filter(f => f != row);

            this.setState({tmp: Math.random()});
        }}>
            <i className={'fa fa-minus'}></i>
            حذف
        </Button>
    }


    newRecord() {
        if (!this.props.parent.selected.UrlRoutes) {
            this.props.parent.selected.UrlRoutes = [];
        }


        this.props.parent.selected.UrlRoutes.push({
            urlRoute: this.state.urlRoute,
            type: this.state.type,
        })


        this.setState({
            urlRoute: null,
            type: null
        });


    }

    render() {
        return (
            <>
                <Row>

                    <Col>
                        <label>آدرس صفحه</label>
                        <InputMask
                            mask="www.a*"
                            value={this.state.urlRoute}
                            onChange={(e) => this.setState({urlRoute: e.value})}
                        />

                    </Col>
                    <Col>
                        <label>نحوه اعمال</label>
                        <Dropdown value={this.state.type}
                                  options={this.types}
                                  onChange={this.onCityChange}
                                  optionLabel="name"
                                  placeholder="نوع اعمال"/>
                    </Col>
                    <Col>

                        <Button onClick={() => {

                            this.newRecord();
                        }}>

                            <i className={'fa fa-plus'}></i>
                            <span>افزودن الگوی آدرس جدید</span>
                        </Button>

                    </Col>

                </Row>
                <hr/>

                <Row>
                    <Col>
                        <DataTable value={this.props.parent.selected.UrlRoutes}
                                   paginator
                                   paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                                   currentPageReportTemplate="Showing {first} to {last} of {totalRecords}" rows={10}
                                   rowsPerPageOptions={[10, 20, 50]}
                        >
                            <Column field="urlRoute" header="آدرس"></Column>
                            <Column body={this.typeTemplate} header="نوع"></Column>
                            <Column body={this.actionTemplate} header="عملیات"></Column>
                        </DataTable>
                    </Col>
                </Row>

            </>
        );
    }
}

export default AddUrlRoute;