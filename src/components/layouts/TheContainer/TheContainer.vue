<template>
  <div class="container">
    <div class="content__tittle">
      <div class="title">Nguyên vật liệu</div>
      <button class="btn">
        <i class="fa-solid fa-bullhorn icon__horn"></i>Phản hồi
      </button>
    </div>
    <div class="content__table">
      <LoadingVue v-show="isLoad"/>
      <div class="table__option">
        <div class="label color__black" @click="showForm(true)">
          <i class="fa-solid fa-file-circle-plus icon__add"></i>Thêm
        </div>
        <div class="label color__black" @click="onDuplicateClick()">
          <i class="fa-regular fa-copy icon__duplicate"></i>Nhân Bản
        </div>
        <div class="label color__black" @click="onUpdateClick()">
          <i class="fa-solid fa-pen-to-square icon__add"></i>Sửa
        </div>
        <div class="label color__black" @click="onDeleteClick(true)">
          <i class="fa-solid fa-xmark icon__delete"></i>Xóa
        </div>
        <div class="label color__black">
          <i class="fa-solid fa-arrows-rotate icon__add"></i>Nạp
        </div>
      </div>
      <table class="table__data">
        <tr>
          <th>
            Mã nguyên vật liệu
            <div class="table__text--search">
              <SelectionBox
                ref="MaterialCode"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('MaterialCode')"
              />
              <input
                v-model="MaterialCode"
                type="text"
                class="input__search"
                @keydown.enter="getFillterOption('MaterialCode')"
              />
            </div>
          </th>
          <th>
            Tên nguyên vật liệu
            <div class="table__text--search">
              <SelectionBox
                ref="MaterialName"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('MaterialName')"
              />
              <input
                type="text"
                class="input__search"
                v-model="MaterialName"
                @keydown.enter="getFillterOption('MaterialName')"
              />
            </div>
          </th>
          <th>
            Tính chất
            <div class="table__text--search">
              <SelectionBox
                ref="Feature"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('Feature')"
              />
              <input
                type="text"
                class="input__search"
                v-model="Feature"
                @keydown.enter="getFillterOption('Feature')"
              />
            </div>
          </th>
          <th>
            ĐVT
            <div class="table__text--search">
              <SelectionBox
                ref="UnitName"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('UnitName')"
              />
              <input
                type="text"
                class="input__search"
                v-model="UnitName"
                @keydown.enter="getFillterOption('UnitName')"
              />
            </div>
          </th>
          <th>
            Nhóm nguyên vật liệu
            <div class="table__text--search">
              <SelectionBox
                ref="CategoryName"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('CategoryName')"
              />
              <input
                type="text"
                class="input__search"
                v-model="CategoryName"
                @keydown.enter="getFillterOption('CategoryName')"
              />
            </div>
          </th>
          <th>
            Ghi chú
            <div class="table__text--search">
              <SelectionBox
                ref="Description"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('Description')"
              />
              <input
                type="text"
                class="input__search"
                v-model="Description"
                @keydown.enter="getFillterOption('Description')"
              />
            </div>
          </th>
          <th>
            Ngừng theo dõi
            <SelectionBox
              :isSelectShow="true"
              :div_class="'table__text--search'"
              :input_class="'select__input col_width_100'"
              :option="statusOption"
              :optionType="statusType"
              :value="statusOption[0].text"
              :optionID="statusOption[0].id"
              @Status="setStatus"
            />
          </th>
        </tr>
        <tr
          v-for="item in materialList"
          :key="item.materialID"
          @click="getMaterialSelected(item)"
          @dblclick="onUpdateClick()"
          :class="{ row__selected: MaterialID == item.materialID }"
        >
          <td>{{ item.materialCode }}</td>
          <td>{{ item.materialName }}</td>
          <td>{{ item.feature }}</td>
          <td>{{ item.unitName }}</td>
          <td>{{ item.categoryName }}</td>
          <td>{{ item.description }}</td>
          <td class="checkbox__col">
            <input
              type="checkbox"
              :checked="item.status == 2"
              :value="item.status"
            />
          </td>
        </tr>
      </table>
    </div>
    <MaterialPaging
      :totalRecord="totalPageRecord"
      @LoadData="loadData"
      :totalPage="totalPage"
      :materialFillter="materialFillter"
      v-model:pageSize = "pageSize"
    />
    <MaterialForm
      v-if="isShowForm"
      :showForm="showForm"
      @LoadData="loadData"
      :MaterialID="MaterialID"
      :ActionType="actionType"
    />
  </div>
  <PopUp
    :message="message"
    v-if="isShowPopUp"
    @onYesClick="deleteMaterial"
    :closePopUp="onDeleteClick"
    :PopUpType="popUpType"
  />
</template>
<style scoped>
@import url(container.css);
</style>
<script>
import axios from "axios";
import { MISAEnum } from "../../../js/Enum.js";
import { Resources } from "../../../js/Resources.js";
import MaterialForm from "../../base/Form/MaterialForm.vue";
import MaterialPaging from "../../base/Paging/MaterialPaging.vue";
import SelectionBox from "../../base/SelectionBox/SelectionBox.vue";
import PopUp from "../../base/PopUp/ThePopUp.vue";
import LoadingVue from "@/components/base/Loading/LoadData.vue";
export default {
  name: "TheContainer",
  components: { MaterialForm, MaterialPaging, SelectionBox, PopUp, LoadingVue },
  data() {
    return {
      isLoad:false, // ẩn hiện loading
      pageSize:10, // số bản ghi hiển thị trong 1 trang
      message: "", // thông tin của pop up
      popUpType: 0, // loại pop up được hiển thị
      isShowPopUp: false, // ẩn hiện pop up
      isShowForm: false, //hiển thị form thông tin nguyên liệu,
      isUpdate: false, // check trạng thái khi nào update
      MaterialID: "", //id nguyên vật liệu
      MaterialCode: "", // mã nguyên vật liệu
      MaterialName: "", // tên nguyên vật liệu
      Feature: "", // tính chất
      UnitName: "", // tên đơn vị
      CategoryName: "", // tên nhóm nguyên vật liệu
      Description: "", // mô tả
      material: {}, // nguyên vật liệu
      materialFillter: {
        MaterialCode: "",
        MaterialName: "",
        Feature: "",
        UnitName: "",
        CategoryName: "",
        Description: "",
        Status: 1,
      }, //danh sách các thuộc tính lọc nguyên vật liệu
      materialList: [], //danh sách nguyên vật liệu,
      totalPageRecord: 0, // tổng số bản ghi được tìm thấy
      totalPage: 0, // tổng số trang
      statusOption: [
        {
          id: 1,
          value: MISAEnum.StatusOption.Using.value,
          text: MISAEnum.StatusOption.Using.text,
        },
        {
          id: 2,
          value: MISAEnum.StatusOption.NotUsing.value,
          text: MISAEnum.StatusOption.NotUsing.text,
        },
      ], // list trạng thái
      statusType: MISAEnum.OptionType.Status, //loại trạng thái
      fillterOption: [
        {
          id: 1,
          value: MISAEnum.FillterOption.Contain.value,
          text: MISAEnum.FillterOption.Contain.text,
        },
        {
          id: 2,
          value: MISAEnum.FillterOption.Equal.value,
          text: MISAEnum.FillterOption.Equal.text,
        },
        {
          id: 3,
          value: MISAEnum.FillterOption.StartWidth.value,
          text: MISAEnum.FillterOption.StartWidth.text,
        },
        {
          id: 4,
          value: MISAEnum.FillterOption.EndWidth.value,
          text: MISAEnum.FillterOption.EndWidth.text,
        },
        {
          id: 5,
          value: MISAEnum.FillterOption.NotContain.value,
          text: MISAEnum.FillterOption.NotContain.text,
        },
      ],// lựa chọn lọc 
      fillterType: MISAEnum.OptionType.Fillter, //loại lọc muốn thực hiện
      fillterText: MISAEnum.FillterOption.Contain.value, //text muốn search
      actionType: 0, // loại chức năng muôn thực hiện
    };
  },
  methods: {
    /**
     *Hàm mở form
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    showForm(isShow) {
      this.isShowForm = isShow;
      this.MaterialID = "";
      this.actionType = MISAEnum.ActionType.Insert;
    },
    /**
     *Hàm load danh sách nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    loadData(pageSize, pageNumber, materialFillter) {
      this.isLoad = true;
      axios
        .get(
          Resources.DOMAIN +
            Resources.API_VER +
            Resources.MATERIAL_PATH +
            `/Fillter?MaterialCode=${materialFillter.MaterialCode}&MaterialName=${materialFillter.MaterialName}&Feature=${materialFillter.Feature}&UnitName=${materialFillter.UnitName}&CategoryName=${materialFillter.CategoryName}&Description=${materialFillter.Description}&Status=${materialFillter.Status}&pageSize=${pageSize}&pageNumber=${pageNumber}`
        )
        .then((response) => {
          if (response) {
            this.materialList = response.data.data;
            this.MaterialID  = response.data.data[0].materialID;
            this.material = response.data.data[0];
            this.totalPage = response.data.totalPage;
            this.totalPageRecord = response.data.totalRecord;
            this.currentPage = response.data.currentPage;
             this.isLoad = false;
            // this.setPagingValue();
            // this.isData = response.data.data.length > 0 ? true : false;
            // this.showPaging = response.data.data.length > 0 ? true : false;
          }
        })
        .catch((error) => {
          console.log(error.response);
          // this.errorMsg = [];
          // this.popUpType = MISAEnum.PopUpType.Alert;
          // this.showPopUp = true;
          // this.errorMsg.push({
          //   msg: Resources.UNDEFINED_ERROR,
          //   tittle: MISAEnum.Status.Error,
          // });
        });
    },
     /**
     *Hàm set trạng thái
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    setStatus(data) {
      this.materialFillter.Status = data;
      this.loadData(10, 1, this.materialFillter);
    },
     /**
     *Hàm lấy ra lựa chọn lọc
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    getFillterOption(value) {
      if (this.$refs[value].fillterText != "+") {
        this.materialFillter[value] =
          this.$refs[value].fillterText + this[value];
      } else {
        this.materialFillter[value] = "%2B" + this[value];
      }
      this.loadData(this.pageSize, 1, this.materialFillter);
    },
     /**
     *Hàm lấy thông tin nguyên vật liệu khi click
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    getMaterialSelected(material) {
      this.MaterialID = material.materialID;
      this.material = material;
    },
     /**
     *Hàm mở form khi click button sửa
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onUpdateClick() {
      if (this.MaterialID) {
        this.isShowForm = true;
        this.actionType = MISAEnum.ActionType.Update;
      }
    },
 /**
     *Hàm thực hiện chức năng nhân bản
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onDuplicateClick() {
      if (this.MaterialID) {
        this.isShowForm = true;
        this.actionType = MISAEnum.ActionType.Duplicate;
      }
    },
     /**
     *Hàm xóa nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    deleteMaterial() {
      axios
        .delete(
          Resources.DOMAIN +
            Resources.API_VER +
            Resources.MATERIAL_PATH +
            `/${this.MaterialID}`
        )
        .then((response) => {
          if (response.data == this.MaterialID) {
            this.loadData(this.pageSize, 1, this.materialFillter);
          }
        })
        .catch((error) => {
          console.log(error.response);
        });
    },
     /**
     *Hàm mở form confirm trước khi xóa
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onDeleteClick(isShow) {
      this.message = [];
      this.isShowPopUp = isShow;
      this.message.push({
        msg: `Bạn có chắc muốn xóa Nguyên vật liệu << ${this.material.materialCode} - ${this.material.materialName}>> không?`,
      });
      this.popUpType = MISAEnum.PopUpType.Alert;
    },
  },
  created() {
    this.loadData(10, 1, this.materialFillter);
  },
};
</script>
