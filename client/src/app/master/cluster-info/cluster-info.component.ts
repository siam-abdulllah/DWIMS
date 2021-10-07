import { ClusterMstInfo, IClusterMstInfo,ClusterDtlInfo, IClusterDtlInfo } from '../../shared/models/clusterInfo';
import { GenericParams } from '../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { IRegion } from 'src/app/shared/models/location';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-cluster-info',
  templateUrl: './cluster-info.component.html',
  styleUrls: ['./cluster-info.component.scss']
})
export class ClusterInfoComponent implements OnInit {
  
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('clusterMstSearchModal', { static: false }) clusterMstSearchModal: TemplateRef<any>;
  clusterMstSearchodalRef: BsModalRef;
  genParams: GenericParams;
  regions: IRegion[];
  searchText = '';
  clusterMstInfo: IClusterMstInfo[];
  clusterDtlInfo: IClusterDtlInfo[];
  totalCount = 0;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(public masterService: MasterService, 
    private router: Router, private toastr: ToastrService, private modalService: BsModalService, private SpinnerService: NgxSpinnerService) { }

  ngOnInit(): void {
    this.resetForm();
    this.getRegion();
  }
  getRegion() {
    this.masterService.getRegion().subscribe(response => {
      this.regions = response as IRegion[];
    }, error => {
      console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    if (this.masterService.clusterMstFormData.id == 0)
      this.insertClusterMst(form);
    else
      this.updateClusterMst(form);
  }
  insertClusterMst(form: NgForm) {
    this.masterService.insertClusterMst().subscribe(
      res => {
        this.masterService.clusterMstFormData = res as IClusterMstInfo;
        this.toastr.success('Submitted successfully', 'Cluster Info')
      },
      err => { console.log(err); }
    );
  }

  updateClusterMst(form: NgForm) {
    this.masterService.updateClusterMst().subscribe(
      res => {
        this.masterService.clusterMstFormData = res as IClusterMstInfo;
        this.toastr.info('Updated successfully', 'Cluster Info')
      },
      err => { console.log(err); }
    );
  }
  addRegion() {
    debugger;
    if (this.masterService.clusterMstFormData.id == 0) {
      this.toastr.warning('Please Insert Cluster Data First', 'Cluster Info');
      return false;
    }
    
    this.masterService.clusterDtlFormData.mstId = this.masterService.clusterMstFormData.id;
    if (this.masterService.clusterDtlFormData.id == 0) {
    if (this.clusterDtlInfo.length > 0) {
      for (var i = 0; i < this.clusterDtlInfo.length; i++) {
        if (this.clusterDtlInfo[i].regionCode == this.masterService.clusterDtlFormData.regionCode) {
          this.toastr.warning('Region Already Exist', 'Cluster');
          return false;
        }
      }
    }
    for (var i = 0; i < this.regions.length; i++) {
      if (this.regions[i].regionCode == this.masterService.clusterDtlFormData.regionCode) {
        this.masterService.clusterDtlFormData.regionName=this.regions[i].regionName;
        break;
      }
    }
      this.masterService.insertClusterDtl().subscribe(
        res => {
          this.masterService.clusterDtlFormData = new ClusterDtlInfo();
          this.getClusterDtlList();
          this.toastr.info('Insert successfully', 'Cluster Info')
        },
        err => { console.log(err); }
      );
    }
    else { 
      for (var i = 0; i < this.regions.length; i++) {
      if (this.regions[i].regionCode == this.masterService.clusterDtlFormData.regionCode) {
        this.masterService.clusterDtlFormData.regionName=this.regions[i].regionName;
        break;
      }
    }
      this.masterService.updateClusterDtl().subscribe(
        res => {
          this.masterService.clusterDtlFormData = new ClusterDtlInfo();
          this.getClusterDtlList();
          this.toastr.info('Update successfully', 'Cluster Info')
        },
        err => { console.log(err); }
      );
    }
  }
  getClusterMstList(){
    this.SpinnerService.show(); 
    this.masterService.getClusterMstList().subscribe(response => {
      this.SpinnerService.hide(); 
      this.clusterMstInfo = response.data;
      this.totalCount = response.count;
      if (this.clusterMstInfo.length>0) {
        this.openClusterMstSearchModal(this.clusterMstSearchModal);
      }
      else {
        this.toastr.warning('No Data Found');
      }
     }, error => {
      this.SpinnerService.hide();
         console.log(error);
    });
  }
  openClusterMstSearchModal(template: TemplateRef<any>) {
    this.clusterMstSearchodalRef = this.modalService.show(template, this.config);
  }
  getClusterDtlList(){
    this.masterService.getClusterDtlList(this.masterService.clusterMstFormData.id).subscribe(response => {
      debugger;
      this.clusterDtlInfo = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }

  onPageChanged(event: any){
    const params = this.masterService.getGenParams();
    if (params.pageNumber !== event)
    {
      params.pageNumber = event;
      this.masterService.setGenParams(params);
      this.getClusterMstList();
    }
  }
  
  onSearch(){
    const params = this.masterService.getGenParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.masterService.setGenParams(params);
    this.getClusterMstList();
  }

  resetSearch(){
    this.searchText = '';
}

  resetPage(form: NgForm) {
    form.form.reset();
    this.masterService.clusterMstFormData = new ClusterMstInfo();
    this.masterService.clusterDtlFormData = new ClusterDtlInfo();
    this.clusterDtlInfo=[];
    this.clusterMstInfo=[];
  }
  resetForm() {
    this.masterService.clusterMstFormData = new ClusterMstInfo();
    this.masterService.clusterDtlFormData = new ClusterDtlInfo();
    this.clusterDtlInfo=[];
    this.clusterMstInfo=[];
  }
  
  selectClusterMst(selectedRecord: IClusterMstInfo) {
    this.masterService.clusterMstFormData = Object.assign({}, selectedRecord);
    this.getClusterDtlList();
    this.clusterMstSearchodalRef.hide()
  }
  populateDtlsForm(selectedRecord: IClusterDtlInfo) {
    this.masterService.clusterDtlFormData = Object.assign({}, selectedRecord);

  }
}
