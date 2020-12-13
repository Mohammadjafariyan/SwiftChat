import React, { Component } from "react";
import { Card, Row, Col, ProgressBar } from "react-bootstrap";

export default class Rating extends Component {
  render() {
      debugger;
    let r0 = this.props.rating.RatingList[0];
    let r1 = this.props.rating.RatingList[1];
    let r2 = this.props.rating.RatingList[2];
    let r3 = this.props.rating.RatingList[3];
    let r4 = this.props.rating.RatingList[4];
    let r5 = this.props.rating.RatingList[5];

    var arr=[];
    for (let i = 0; i < this.props.rating.RatingList.length; i++) {
        const element = this.props.rating.RatingList[i];
        arr.push(element.Count)
    }
    var _max = arrayMax(arr);
    return (
      <div>
        <Card bg="light" border="primary">
          <Card.Header>آمار امتیازات دریافت شده</Card.Header>
          <Card.Body>
            {this.props.rating && (
              <Row>
                <Col>
                  <h4>میانگین امتیاز : {this.props.rating.MeanScore}</h4>
                </Col>
                <Col>
                  <h4>نظرات : {this.props.rating.CommentsCount}</h4>
                </Col>
                <Col>
                  {this.props.rating.RatingList && (
                    <>
                      <Row>
                        <Col>6/6</Col>
                        <Col>
                          <ProgressBar
                            striped
                            variant="success"
                            now={(100 * r0.Count) / _max}
                            label={r0.Count}
                          />
                        </Col>
                        <Col></Col>
                      </Row>
                      <Row>
                        <Col>5/6</Col>
                        <Col>
                          <ProgressBar
                            striped
                            variant="info"
                            now={(100 * r1.Count) / _max}
                            label={r1.Count}
                          />
                        </Col>
                        <Col></Col>
                      </Row>
                      <Row>
                        <Col>4/6</Col>
                        <Col>
                          <ProgressBar
                            striped
                            variant="danger"
                            now={(100 * r2.Count) / _max}
                            label={r2.Count}
                          />
                        </Col>
                        <Col></Col>
                      </Row>
                      <Row>
                        <Col>3/6</Col>
                        <Col>
                          <ProgressBar
                            striped
                            variant="warning"
                            now={(100 * r3.Count) / _max}
                            label={r3.Count}
                          />
                        </Col>
                        <Col></Col>
                      </Row>
                      <Row>
                        <Col>2/6</Col>
                        <Col>
                          <ProgressBar
                            striped
                            variant="dark"
                            now={(100 * r4.Count) / _max}
                            label={r4.Count}
                          />
                        </Col>
                        <Col></Col>
                      </Row>
                      <Row>
                        <Col>1/6</Col>
                        <Col>
                          <ProgressBar
                            striped
                            variant="Secondary"
                            now={(100 * r5.Count) / _max}
                            label={r5.Count}
                          />
                        </Col>
                        <Col></Col>
                      </Row>
                     
                    </>
                  )}
                </Col>
              </Row>
            )}
          </Card.Body>
        </Card>
      </div>
    );
  }
}

function arrayMax(arr) {
  if (!arr || arr.length == 0) return 0;

  return arr.reduce(function (p, v) {
    return p > v ? p : v;
  });
}
