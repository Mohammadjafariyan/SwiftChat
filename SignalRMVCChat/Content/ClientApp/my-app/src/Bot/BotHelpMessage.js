import React, {Component} from 'react';
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {Alert, Card} from "react-bootstrap";

class BotHelpMessage extends Component {
    render() {
        return (
            <>
                <Row>
                    <Col>
                        <Card>
                            <Card.Body>
                                <Card.Text>
                                    <Alert variant={'warning'}>
                                        <div className="cap-plugin-content-box-inner">
                                            <p className="cap-font-sans-regular"><span
                                                className="cap-font-sans-semibold">پلاگین Bot به شما امکان می دهد <b>پشتیبانی خود را به صورت خودکار انجام دهید</b>. سناریوهای ربات را ایجاد کرده و نمودار رویدادها ، شرایط ، اقدامات و نتایج را برای ربات خود پیکربندی کنید. حتی می توانید از پلاگین Bot برای ایجاد فرم ها و ذخیره داده های فرم در پروفایل های کاربر خود استفاده کنید.
                                                </span>
                                            </p>
                                            <p className="cap-font-sans-regular">قبل از انتشار ، می توانید نمودار نمودار سناریوی خود را با استفاده از سازنده Bot Scenario بصورت زنده امتحان کنید.
                                            </p>

                                            <p className="cap-font-sans-bold help-title">نحوه کار ربات بشرح زیر است:</p>

                                            <ol className="cap-font-sans-semibold help-list" style={{textAlign:'right'}}>
                                                <li>یک سناریو ایجاد کنید</li>
                                                <li>یک گراف برای سناریو بسازید</li>
                                                <li>ربات خود را قبل از انتشار بصورت زنده تست کنید</li>
                                                <li>ربات خود را انتشار دهید تا توسط کاربران دیگر مورد استفاده قرار گیرد</li>
                                            </ol>

                                            <div className="help-screen-wrap">
                                                <div className="help-screen"></div>
                                            </div>
                                        </div>

                                        <hr/>
                                        در این بخش ربات ها تعریف ، ویرایش و حذف می شوند
                                    </Alert>
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    </Col>
                </Row>
                <br/>

                <Row>
                </Row>
            </>
        );
    }
}

export default BotHelpMessage;