import React, {Component} from 'react';
import {TabMenu} from "primereact/tabmenu";
import Row from "react-bootstrap/cjs/Row";
import Col from "react-bootstrap/cjs/Col";

class CompaignFilter extends Component {
    constructor(props) {
        super(props);

        this.items =  [
            {label: 'کل', icon: 'pi pi-fw pi-home'},
            {label: 'اتوماتیک', icon: 'pi pi-fw pi-calendar'},
            {label: 'دستی', icon: 'pi pi-fw pi-pencil'},
        ];

        this.filters =  [
            {label: 'در حال ارسال', icon: 'pi pi-fw pi-home'},
            {label: 'آماده ارسال', icon: 'pi pi-fw pi-calendar'},
            {label: 'متوقف', icon: 'pi pi-fw pi-pencil'},
            {label: 'پیکربندی نشده', icon: 'pi pi-fw pi-pencil'},
            {label: 'ارسال شده', icon: 'pi pi-fw pi-pencil'},
        ];
    }

    render() {
        return (
            <Row>
                <Col>
                    <TabMenu model={this.items} />
                </Col>
                <Col>
                    <TabMenu model={this.filters} />
                </Col>
            </Row>
        );
    }
}

export default CompaignFilter;